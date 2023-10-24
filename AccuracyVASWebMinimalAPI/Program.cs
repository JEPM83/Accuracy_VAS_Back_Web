using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccuracyModel.Printer;
using AccuracyModel.General;
using AccuracyModel.Vas;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using System.Xml;
using AccuracyModel.Security;
using Microsoft.Extensions.Logging;
// 
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);

string connString = builder.Configuration.GetConnectionString("SQLDBConnection");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnetClaimAuthorization", Version = "v1" }); 
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingrese el token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "1WT",
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "access-control-allow-headers",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200",
                                              "http://www.contoso.com").AllowAnyMethod();
                      });
    options.AddPolicy(name: "Access-Control-Allow-Origin",
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200",
                                              "http://www.contoso.com").AllowAnyMethod();
                      });
});

// Configurar la autenticación con token
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = key
    };
});
// Agregar la autorización
builder.Services.AddAuthorization();

var app = builder.Build(); 
app.UseSwagger();
app.UseSwaggerUI(); 
app.UseAuthentication(); // Agregar autenticación con token antes de Swagger
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader()); 
// Agregar autorización con token después de la autenticación
app.UseAuthorization();
#region Security
app.MapPost("/accuracy/vas/api/v1/LoginWeb", [AllowAnonymous] async ([FromBody] UserRequest obj, HttpResponse response) =>
{
    AccuracyBussiness.SecurityBL.SecurityWebBL poBL = new AccuracyBussiness.SecurityBL.SecurityWebBL();
    string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    List<UserResponse> resp = poBL.Login(obj, HostGroupId, connString);
    if (resp[0].status == "1")
    //if (resp[0].mensaje == "EL USUARIO YA TIENE UNA SESIÓN ACTIVA EN OTRO DISPOSITIVO" || resp[0].mensaje == "LAS CREDENCIALES ESTAN INCORRECTAS" || resp[0].mensaje == "NO EXISTE EL USUARIO EN LA BASE DE DATOS, VERIFIQUE.")
    {
        var modalResponse = new
        {
            title = "Warning",
            message = resp[0].mensaje,
            type = 3//resp[0].mensaje == "EL USUARIO YA TIENE UNA SESIÓN ACTIVA EN OTRO DISPOSITIVO" ? 1 : resp[0].mensaje == "LAS CREDENCIALES ESTAN INCORRECTAS" ? 2 : 3
        };

        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.StatusCode = StatusCodes.Status400BadRequest;
        return Results.Json(modalResponse);
    }

    var claims = new List<Claim>(); 
    foreach (var userResponse in resp)
    {
        claims.Add(new Claim("usuario", userResponse.usuario));
        claims.Add(new Claim("nombres", userResponse.nombres ?? ""));
        claims.Add(new Claim("apellidos", userResponse.apellidos ?? ""));
        claims.Add(new Claim("guid_sesion", userResponse.guid_sesion));
        claims.Add(new Claim("linea_produccion", userResponse.linea_produccion));
        claims.Add(new Claim("almacen", "01"));
        claims.Add(new Claim("tipo", userResponse.tipo.ToString()));
    } 
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])); 
    // Crear el token JWT
    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(12),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
    );
    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
    var tokenResponse = new
    {
        Token = jwtToken,
        usuario = resp[0].usuario,
        nombres = resp[0].nombres,
        apellidos = resp[0].apellidos,
        estado_sesion = resp[0].estado_sesion,
        guid_sesion = resp[0].guid_sesion,
        status = resp[0].status,
        mensaje = resp[0].mensaje,
        ruta = resp[0].ruta,
        tipo = resp[0].tipo
    }; 
    response.Headers.Add("Access-Control-Allow-Origin", "*");
    return Results.Ok(tokenResponse);
})
.Accepts<UserRequest>("application/json")
.Produces(StatusCodes.Status200OK)
.WithName("LoginWeb")
.WithTags("Security");
//-------------------   
app.MapPost("/accuracy/vas/api/v1/GetWarehouseUser",
    [AllowAnonymous] async ([FromBody] UserWarehouseRequest obj, HttpContext context) =>
    {
        // Aquí verificamos la autenticación del usuario con token
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.SecurityBL.SecurityWebBL poBL = new AccuracyBussiness.SecurityBL.SecurityWebBL();

                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.SecurityBL.SecurityWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "1"
                    };
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    return Results.Json(errorResponse);
                }
                // Validar si poBL no trae datos
                if (poBL == null)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "La instancia poBL no contiene datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                } 
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<UserWarehouseResponse> resp = poBL.WarehouseUser(obj, HostGroupId, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error  
                var errorResponse = new
                {
                    title = "Error",
                    message = "Token invalido",
                    type = "1"

                };
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = "Error al validar token",
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autorizado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<UserWarehouseRequest>("application/json")
    .Produces<UserWarehouseRequest>(StatusCodes.Status200OK)
    .WithName("GetWarehouseUser")
    .WithTags("Security");
///-----------
app.MapPost("/accuracy/vas/api/v1/LogoutWeb",
    [AllowAnonymous] async ([FromBody] UserRequest obj, HttpContext context) =>
    {
        AccuracyBussiness.SecurityBL.SecurityWebBL poBL = new AccuracyBussiness.SecurityBL.SecurityWebBL();
        string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        List<UserResponse> resp = poBL.Logout(obj, HostGroupId, connString);

        var response = new
        {   message = "Sesión cerrada correctamente",
            data = resp
        }; 
        // Configurar el encabezado de respuesta para permitir el origen cruzado
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*"); 
        // Devolver la respuesta con el mensaje de sesión cerrada y los datos adicionales
        return Results.Ok(response);
    })
    .Accepts<UserRequest>("application/json")
    .Produces(StatusCodes.Status200OK)
    .WithName("LogoutWeb")
    .WithTags("Security");
/**/
#endregion
#region PRINTER WEB
//
app.MapPost("/accuracy/vas/api/v1/GetPrinter",
    [AllowAnonymous] async ([FromBody] PrinterRequestWeb obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.PrinterBL.PrinterWebBL poBL = new AccuracyBussiness.PrinterBL.PrinterWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<PrinterBodyWeb> resp = poBL.SP_PRINTER_WEB_GET_CONFIG(obj,connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PrinterRequestWeb>("application/json")
    .Produces<PrinterRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetPrinter")
    .WithTags("Printer");
//
//
app.MapPost("/accuracy/vas/api/v1/PostInsertLPNPrinter",
    [AllowAnonymous] async ([FromBody] RequestLPNWeb obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.PrinterBL.PrinterWebBL poBL = new AccuracyBussiness.PrinterBL.PrinterWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<BodyLPNWeb> resp = poBL.SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestLPNWeb>("application/json")
    .Produces<RequestLPNWeb>(StatusCodes.Status200OK)
    .WithName("PostInsertLPNPrinter")
    .WithTags("Printer");
//
//
app.MapPost("/accuracy/vas/api/v1/GetCorrelativeLPN",
    [AllowAnonymous] async ([FromBody] RequestCorrelativoLPNWeb obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.PrinterBL.PrinterWebBL poBL = new AccuracyBussiness.PrinterBL.PrinterWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                BodyCorrelativoLPNWeb resp = poBL.SP_PRINTER_WEB_GET_LPN_CORRELATIVE(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestCorrelativoLPNWeb>("application/json")
    .Produces<RequestCorrelativoLPNWeb>(StatusCodes.Status200OK)
    .WithName("GetCorrelativeLPN")
    .WithTags("Printer");
//
#endregion
#region GENERAL
//
app.MapPost("/accuracy/vas/api/v1/GetAttributeObject",
    [AllowAnonymous] async ([FromBody] GeneralObjetoRequestWeb obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.General.GeneralWebBL poBL = new AccuracyBussiness.General.GeneralWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<GeneralObjetotBodyWeb> resp = poBL.SP_KPI_WEB_GET_LISTA_ATRIBUTO_OBJETO(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "3"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<GeneralObjetoRequestWeb>("application/json")
    .Produces<GeneralObjetoRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetAttributeObject")
    .WithTags("General");
//
#endregion
#region VAS
//
app.MapPost("/accuracy/vas/api/v1/GetLineProduction",
    [AllowAnonymous] async ([FromBody] LineRequest obj, HttpContext context) =>
    {
            try
            {
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();
                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<LineResponse> resp = poBL.GET_PRODUCTION_LINE_BY_TERMINAL(obj, connString);
                if (resp.Count == 0) {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Sin registros",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else if (resp[0].estado == 1) {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = resp[0].mensaje.ToString(),
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }      
                else {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
    })
    .Accepts<LineRequest>("application/json")
    .Produces<LineRequest>(StatusCodes.Status200OK)
    .WithName("GetLineProduction")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetOrder",
    [AllowAnonymous] async ([FromBody] OrderPedidoRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<OrderPedidoResponse> resp = poBL.GET_ORDER_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registro de pedidos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OrderPedidoRequest>("application/json")
    .Produces<OrderPedidoRequest>(StatusCodes.Status200OK)
    .WithName("GetOrder")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostNotifyOrder",
    [AllowAnonymous] async ([FromBody] NotifyOrderRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<NotifyOrderResponse> resp = poBL.POST_NOTIFY_ORDER_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro notificacion de orden",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<NotifyOrderRequest>("application/json")
    .Produces<NotifyOrderRequest>(StatusCodes.Status200OK)
    .WithName("PostNotifyOrder")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetOrderDetailPicking",
    [AllowAnonymous] async ([FromBody] OrderPedidoDetailPickingRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<OrderPedidoDetailPickingResponse> resp = poBL.GET_ORDER_DETAIL_PICKING_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registro de pedidos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OrderPedidoDetailPickingRequest>("application/json")
    .Produces<OrderPedidoDetailPickingRequest>(StatusCodes.Status200OK)
    .WithName("GetOrderDetailPicking")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetOrderDetailTask",
    [AllowAnonymous] async ([FromBody] TaskRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<RootTaskObject> resp = poBL.GET_ORDER_DETAIL_TASK_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registros que cumplan dicha coincidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<TaskRequest>("application/json")
    .Produces<TaskRequest>(StatusCodes.Status200OK)
    .WithName("GetOrderDetailTask")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostStartTask",
    [AllowAnonymous] async ([FromBody] InicioTareaRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<InicioTareaResponse> resp = poBL.POST_START_TASK_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro tarea",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InicioTareaRequest>("application/json")
    .Produces<InicioTareaRequest>(StatusCodes.Status200OK)
    .WithName("PostStartTask")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetPackingDetail",
    [AllowAnonymous] async ([FromBody] PackingdetailRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<PackingdetailResponse> resp = poBL.GET_HU_DETAIL_BY_ORDER_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registros a mostrar",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PackingdetailRequest>("application/json")
    .Produces<PackingdetailRequest>(StatusCodes.Status200OK)
    .WithName("GetPackingDetail")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetLPNvalidate",
    [AllowAnonymous] async ([FromBody] LPNvalidateRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<LPNvalidaResponse> resp = poBL.GET_LPN_VALIDATE_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro tarea",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<LPNvalidateRequest>("application/json")
    .Produces<LPNvalidateRequest>(StatusCodes.Status200OK)
    .WithName("GetLPNvalidate")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostLPNSKU",
    [AllowAnonymous] async ([FromBody] LPNSKURequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<LPNSKUResponse> resp = poBL.POST_LPN_SKU_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro tarea",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<LPNSKURequest>("application/json")
    .Produces<LPNSKURequest>(StatusCodes.Status200OK)
    .WithName("PostLPNSKU")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostDeleteVas",
    [AllowAnonymous] async ([FromBody] DeleteVasRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<DeleteVasResponse> resp = poBL.POST_DELETE_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro tarea",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<DeleteVasRequest>("application/json")
    .Produces<DeleteVasRequest>(StatusCodes.Status200OK)
    .WithName("PostDeleteVas")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostEndTask",
    [AllowAnonymous] async ([FromBody] FinTareaRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<FinTareaResponse> resp = poBL.POST_END_TASK_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro tarea",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<FinTareaRequest>("application/json")
    .Produces<FinTareaRequest>(StatusCodes.Status200OK)
    .WithName("PostEndTask")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostStartIncidence",
    [AllowAnonymous] async ([FromBody] InicioIncidenciaRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<InicioIncidenciaResponse> resp = poBL.POST_START_INCIDENCE_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InicioIncidenciaRequest>("application/json")
    .Produces<InicioIncidenciaRequest>(StatusCodes.Status200OK)
    .WithName("PostStartIncidence")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostEndIncidence",
    [AllowAnonymous] async ([FromBody] FinIncidenciaRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<FinIncidenciaResponse> resp = poBL.POST_END_INCIDENCE_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<FinIncidenciaRequest>("application/json")
    .Produces<FinIncidenciaRequest>(StatusCodes.Status200OK)
    .WithName("PostEndIncidence")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetListNotify",
    [AllowAnonymous] async ([FromBody] ListaNotificacionRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<ListaNotificacionResponse> resp = poBL.GET_LIST_NOTIFY_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<ListaNotificacionRequest>("application/json")
    .Produces<ListaNotificacionRequest>(StatusCodes.Status200OK)
    .WithName("GetListNotify")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostUpdateNotify",
    [AllowAnonymous] async ([FromBody] ActualizacionNotificacionRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<ActualizacionNotificacionResponse> resp = poBL.POST_UPDATE_NOTIFY_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<ActualizacionNotificacionRequest>("application/json")
    .Produces<ActualizacionNotificacionRequest>(StatusCodes.Status200OK)
    .WithName("PostUpdateNotify")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetPanelLineaProduccion",
    [AllowAnonymous] async ([FromBody] PanelLineaRequest obj, HttpContext context) =>
    {
        //var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        //if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        //{
        //    var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //        ValidAudience = builder.Configuration["Jwt:Audience"],
        //        IssuerSigningKey = key
        //    };

            try
            {
                // Intenta validar el token
                //SecurityToken validatedToken;
                //var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<PanelLineaResponse> resp = poBL.GET_PANEL_LINEA_PRODUCCION_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        //}
        //else
        //{
        //    // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
        //    var errorResponse = new
        //    {
        //        title = "Warning",
        //        message = "Usuario no autenticado",
        //        type = "3"
        //    };
        //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //    return Results.Json(errorResponse);
        //}
    })
    .Accepts<PanelLineaRequest>("application/json")
    .Produces<PanelLineaRequest>(StatusCodes.Status200OK)
    .WithName("GetPanelLineaProduccion")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetPanelOrder",
    [AllowAnonymous] async ([FromBody] PanelOrdenRequest obj, HttpContext context) =>
    {
    //    var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

    //    if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
    //    {
    //        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    //        var validationParameters = new TokenValidationParameters
    //        {
    //            ValidateIssuer = true,
    //            ValidateAudience = true,
    //            ValidateLifetime = true,
    //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //            ValidAudience = builder.Configuration["Jwt:Audience"],
    //            IssuerSigningKey = key
    //        };

            try
            {
                // Intenta validar el token
                //SecurityToken validatedToken;
                //var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<PanelOrdenResponse> resp = poBL.GET_PANEL_ORDER_PRODUCCION_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro incidencia",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        //}
        //else
        //{
        //    // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
        //    var errorResponse = new
        //    {
        //        title = "Warning",
        //        message = "Usuario no autenticado",
        //        type = "3"
        //    };
        //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //    return Results.Json(errorResponse);
        //}
    })
    .Accepts<PanelOrdenRequest>("application/json")
    .Produces<PanelOrdenRequest>(StatusCodes.Status200OK)
    .WithName("GetPanelOrder")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetOrderArc",
    [AllowAnonymous] async ([FromBody] OrdenesVasRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<OrdenesVasResponse> resp = poBL.GET_LIST_ORDER_PRODUCCION_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registros a mostrar",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OrdenesVasRequest>("application/json")
    .Produces<OrdenesVasRequest>(StatusCodes.Status200OK)
    .WithName("GetOrderArc")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetUsersState",
    [AllowAnonymous] async ([FromBody] UsuariosVasRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<UsuariosVasResponse> resp = poBL.GET_LIST_STATE_USERS_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registros a mostrar",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<UsuariosVasRequest>("application/json")
    .Produces<UsuariosVasRequest>(StatusCodes.Status200OK)
    .WithName("GetUsersState")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/GetB2BVas",
    [AllowAnonymous] async ([FromBody] B2BVasRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<B2BVasResponse> resp = poBL.GET_LIST_ORDER_B2B_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No hay registros a mostrar",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return Results.Json(errorResponse);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<B2BVasRequest>("application/json")
    .Produces<B2BVasRequest>(StatusCodes.Status200OK)
    .WithName("GetB2BVas")
    .WithTags("Vas");
//
app.MapPost("/accuracy/vas/api/v1/PostLpnPrinter",
    [AllowAnonymous] async ([FromBody] PrinteLpnRequest obj, HttpContext context) =>
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!StringValues.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.VasBL.VasWebBL poBL = new AccuracyBussiness.VasBL.VasWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener el resultado del registro de inicio de tarea VAS",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<PrinterLpnResponse> resp = poBL.POST_PRINTER_VAS(obj, connString);
                if (resp == null || resp.Count == 0)
                {
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "No se registro impresion",
                        type = "3"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                else
                {
                    if (resp[0].type != "0")
                    {
                        var errorResponse = new
                        {
                            title = resp[0].tittle.ToString(),
                            message = resp[0].message.ToString(),
                            type = resp[0].type.ToString()
                        };
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return Results.Json(errorResponse);
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        return Results.Ok(resp);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "1"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "3"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PrinteLpnRequest>("application/json")
    .Produces<PrinteLpnRequest>(StatusCodes.Status200OK)
    .WithName("PostLpnPrinter")
    .WithTags("Vas");
//
#endregion
app.Run();
