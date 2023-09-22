using AccuracyModel.Outbound;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AccuracyModel.Inventory;
using AccuracyModel.Inbound;
using AccuracyModel.Load;
using AccuracyModel.KPI;
using AccuracyModel.Forecast;
using AccuracyModel.Printer;
using AccuracyModel.General;
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
#region Inbound WEB
app.MapPost("/accuracy/WMS/api/v1/GetOrderInbound",
    [AllowAnonymous] async ([FromBody] PurchaseOrderRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<PurchaseOrderBodyWeb> resp = poBL.SP_INBOUND_WEB_GET_ORDER(obj, HostGroupId, connString);
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
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PurchaseOrderRequestWeb>("application/json")
    .Produces<PurchaseOrderRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetOrderInbound")
    .WithTags("Inbound");

//----
app.MapPost("/accuracy/WMS/api/v1/GetOrderDetailInbound",
    [AllowAnonymous] async ([FromBody] PurchaseOrderDetailRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<PurchaseOrderDetailBodyWeb> resp = poBL.SP_OUTBOUND_WEB_GET_ORDER_DETAIL(obj, HostGroupId, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PurchaseOrderDetailRequestWeb>("application/json")
    .Produces<PurchaseOrderDetailRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetOrderDetailInbound")
    .WithTags("Inbound");
//----
app.MapPost("/accuracy/WMS/api/v1/GetOrderDetailInboundEan",
    [AllowAnonymous] async ([FromBody] PurchaseOrderEanRequest obj, HttpContext context) =>
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
                AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<PurchaseOrderEanResponse> resp = poBL.SP_OUTBOUND_WEB_GET_ORDER_DETAIL_EAN(obj, HostGroupId, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<PurchaseOrderEanRequest>("application/json")
    .Produces<PurchaseOrderEanRequest>(StatusCodes.Status200OK)
    .WithName("GetOrderDetailInboundEan")
    .WithTags("Inbound");
//---
app.MapPost("/accuracy/WMS/api/v1/GetReceipt", //SOLO PARA PAMETECH
    [AllowAnonymous] async ([FromBody] InboundTransactionRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();

                if (poBL == null || string.IsNullOrEmpty(poBL.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<InboundTransactionBodyWeb> resp = poBL.SP_INBOUND_WEB_GET_RECEIPT(obj, HostGroupId, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InboundTransactionRequestWeb>("application/json")
    .Produces<InboundTransactionRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetReceipt")
    .WithTags("Inbound");
//----
app.MapPost("/accuracy/WMS/api/v1/GetReceiptInbound",
    [AllowAnonymous] async ([FromBody] DetalleRecepRequest obj, HttpContext context) =>
    {
        // Validar authorizationHeader que no sea vacio y comienze con Bearer
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
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                // El token es válido, puedes continuar con la lógica de la ruta

                AccuracyBussiness.InboundBL.PurchaseOrderWebBL xyz = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();

                if (xyz == null || string.IsNullOrEmpty(xyz.ToString()))
                {
                    // La instancia de PurchaseOrderWebBL es nula o está vacía, devolver un error de autorización con mensaje JSON personalizado
                    var errorResponse = new
                    {
                        title = "Warning",
                        message = "Error al obtener los datos",
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<DetalleRecepResponse> resp = xyz.SP_INBOUND_WEB_GET_RECEIPT(obj, HostGroupId, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<DetalleRecepRequest>("application/json")
    .Produces<DetalleRecepRequest>(StatusCodes.Status200OK)
    .WithName("GetReceiptInbound")
    .WithTags("Inbound");
 
//---
app.MapPost("/accuracy/WMS/api/v1/GetReceiptLineInbound", [AllowAnonymous] async ([FromBody] DetalleRecep_IDRequest obj, HttpContext context) =>
{
    // Validar authorizationHeader que no sea vacio y comienze con Bearer
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
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // El token es válido, puedes continuar con la lógica de la ruta 
            AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();
            if (poBL == null)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al obtener datos",
                    type = "0"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }

            string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            List<DetalleRecep_IDResponse> resp = poBL.SP_INBOUND_WEB_GET_RECEIPT_LINE(obj, HostGroupId, connString);
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
                type = "0"
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
            type = "0"
        };
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return Results.Json(errorResponse);
    }
})
    .Accepts<DetalleRecep_IDRequest>("application/json")
    .Produces<DetalleRecep_IDRequest>(StatusCodes.Status200OK)
    .WithName("GetReceiptLineInbound")
    .WithTags("Inbound");

//-----
app.MapPost("/accuracy/WMS/api/v1/GetReceiptLineInboundEan", [AllowAnonymous] async ([FromBody] ReceiptLineEanRequest obj, HttpContext context) =>
{
    // Validar authorizationHeader que no sea vacio y comienze con Bearer
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
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // El token es válido, puedes continuar con la lógica de la ruta 
            AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();
            if (poBL == null)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al obtener datos",
                    type = "0"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }

            string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            List<ReceiptLineEanResponse> resp = poBL.SP_INBOUND_WEB_GET_RECEIPT_DETAIL_EAN(obj, HostGroupId, connString);
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Results.Ok(resp);
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                title = "Warning",
                message = ex.Message.ToString(),
                type = "0"
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
            type = "0"
        };
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return Results.Json(errorResponse);
    }
})
    .Accepts<ReceiptLineEanRequest>("application/json")
    .Produces<ReceiptLineEanRequest>(StatusCodes.Status200OK)
    .WithName("GetReceiptLineInboundEan")
    .WithTags("Inbound");

//-----
app.MapPost("/accuracy/WMS/api/v1/PostCloseReceipt", [AllowAnonymous] async ([FromBody] CloseReceipRequest obj, HttpContext context) =>
{
    // Validar authorizationHeader que no sea vacio y comienze con Bearer
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
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // El token es válido, puedes continuar con la lógica de la ruta 
            AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();
            if (poBL == null)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al obtener datos",
                    type = "0"
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }

            string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            CloseReceiptResponse resp = poBL.SP_INBOUND_WEB_POST_SYNC_PURCHASE_ORDER(obj, HostGroupId, connString);
            context.Response.StatusCode = StatusCodes.Status200OK;
            return Results.Ok(resp);
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                title = "Warning",
                message = ex.Message.ToString(),
                type = "0"
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
            type = "0"
        };
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return Results.Json(errorResponse);
    }
})
    .Accepts<CloseReceipRequest>("application/json")
    .Produces<CloseReceipRequest>(StatusCodes.Status200OK)
    .WithName("PostCloseReceipt")
    .WithTags("Inbound");

//-----
app.MapPost("/accuracy/WMS/api/v1/PostInsertPurchaseOrder", [AllowAnonymous] async ([FromBody] InsertReceiptRequest obj, HttpContext context) =>
{
    // Validar authorizationHeader que no sea vacio y comienze con Bearer
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
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // El token es válido, puedes continuar con la lógica de la ruta 
            AccuracyBussiness.InboundBL.PurchaseOrderWebBL poBL = new AccuracyBussiness.InboundBL.PurchaseOrderWebBL();
            if (poBL == null)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al obtener datos",
                    type = 0
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return Results.Json(errorResponse);
            }

            string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            InsertReceiptResponse resp = poBL.SP_INBOUND_WEB_POST_INSERT_PURCHASE_ORDER(obj, HostGroupId, connString);
            if (resp.type == 0)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            else {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var errorResponse = new
                {
                    title = "Warning",
                    message = resp.message,
                    type = resp.type
                };
                return Results.Json(errorResponse);
            }
        }
        catch (Exception ex)
        { 
            var errorResponse = new
            {
                title = "Warning",
                message = ex.Message.ToString(),
                type = "0"
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
            type = "0"
        };
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return Results.Json(errorResponse);
    }
})
    .Accepts<InsertReceiptRequest>("application/json")
    .Produces<InsertReceiptRequest>(StatusCodes.Status200OK)
    .WithName("PostInsertPurchaseOrder")
    .WithTags("Inbound");

//-----
#endregion Inbound WEB
#region Import
app.MapPost("/accuracy/WMS/api/v1/Import",
    [AllowAnonymous] async ([FromBody] LoadImportRequest model, HttpContext context) =>
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
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                string xml = "";
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (XmlWriter writer = XmlWriter.Create(memoryStream))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("orders");
                        foreach (var item in model.data)
                        {
                            writer.WriteStartElement("order");
                            foreach (var element in item)
                            {
                                writer.WriteElementString(element.Key, element.Value.ToString());
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    memoryStream.Position = 0;
                    xml = Encoding.UTF8.GetString(memoryStream.ToArray());
                }

                model.gui = Guid.NewGuid().ToString();
                AccuracyBussiness.LoadBL.LoadImportWebBL poBL = new AccuracyBussiness.LoadBL.LoadImportWebBL();
                //Validar server y instacia
                try
                {
                    poBL = new AccuracyBussiness.LoadBL.LoadImportWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                bool resp = poBL.SP_LOAD_WEB_GET_ORDER_IMPORT(model, xml, connString);

                if (resp)
                {
                    context.Response.StatusCode = 200;
                    return Results.Ok(xml);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    return Results.BadRequest(new { message = "No se logró procesar correctamente la carga masiva.", StatusCode = 400 });
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // No se proporcionó un token válido, devolver un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "Se requiere un token de autorización valido - usuario no autorizado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<LoadImportRequest>("application/json")
    .Produces<LoadImportRequest>(StatusCodes.Status200OK)
    .WithName("Import")
    .WithTags("Load");

//----------- 
#endregion
#region Outbound WEB
app.MapPost("/accuracy/WMS/api/v1/GetOrderOutbound",
    [AllowAnonymous] async ([FromBody] OrderPickingRequest obj, HttpContext context) =>
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
                AccuracyBussiness.OutboundBL.OrderWebBL poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                try
                {
                    poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<OrderPickingBody> resp = poBL.SP_OUTBOUND_WEB_GET_ORDER(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OrderPickingRequest>("application/json")
    .Produces<List<OrderPickingBody>>(StatusCodes.Status200OK)
    .WithName("GetOrderOutbound")
    .WithTags("Outbound");
//---
app.MapPost("/accuracy/WMS/api/v1/GetOrderDetailOutbound",
    [AllowAnonymous] async ([FromBody] Order_DetailRequest obj, HttpContext context) =>
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
                AccuracyBussiness.OutboundBL.OrderWebBL poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<Order_DetailBody> resp = poBL.SP_OUTBOUND_WEB_GET_ORDER_DETAIL(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar el token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<Order_DetailRequest>("application/json")
    .Produces<List<Order_DetailBody>>(StatusCodes.Status200OK)
    .WithName("GetOrderDetailOutbound")
    .WithTags("Outbound");
//--------
app.MapPost("/accuracy/WMS/api/v1/PostOrderUpdateOutbound",
    [AllowAnonymous] async ([FromBody] OrderPickingUpdateRequest obj, HttpContext context) =>
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
                AccuracyBussiness.OutboundBL.OrderWebBL poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<OrderPickingBody> resp = poBL.SP_OUTBOUND_WEB_POST_UPDATE_ORDER(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OrderPickingUpdateRequest>("application/json")
    .Produces<List<OrderPickingBody>>(StatusCodes.Status200OK)
    .WithName("PostOrderUpdateOutbound")
    .WithTags("Outbound");
//---
app.MapPost("/accuracy/WMS/api/v1/GetShipping",
    [AllowAnonymous] async ([FromBody] OutboundTransactionRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.OutboundBL.OrderWebBL poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<OutboundTransactionBodyWeb> resp = poBL.SP_OUTBOUND_WEB_GET_SHIPPING(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = "Error al validar el token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Error",
                message = "Usuario no autorizado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OutboundTransactionRequestWeb>("application/json")
    .Produces<List<OutboundTransactionBodyWeb>>(StatusCodes.Status200OK)
    .WithName("GetShipping")
    .WithTags("Outbound");
//------------

/*
app.MapPost("/accuracy/WMS/api/v1/Detalle_id",
    [AllowAnonymous] async ([FromBody] DetalleRecep_IDRequest obj, HttpResponse response) =>
    {
        AccuracyBussiness.OutboundBL.OrderWebBL poBL = new AccuracyBussiness.OutboundBL.OrderWebBL();
        string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        List<DetalleRecep_IDResponse> resp = poBL.SP_DetalleLineaXidrecepcion(obj, HostGroupId, connString);
        response.StatusCode = 200;
        return Results.Ok(resp);
    })
.Accepts<DetalleRecep_IDRequest>("application/json")
.Produces<DetalleRecep_IDRequest>(StatusCodes.Status200OK)
.WithName("Detalle_id").WithTags("Outbound");*/
/**/
#endregion
#region Inventory WEB
app.MapPost("/accuracy/WMS/api/v1/GetTransactionLog",
    [AllowAnonymous] async ([FromBody] InventoryTransactionRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<InventoryTransactionBodyWeb> resp = poBL.SP_INVENTORY_WEB_GET_TRANSACTION_LOG(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Error",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Error",
                message = "usuario no autorizado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InventoryTransactionRequestWeb>("application/json")
    .Produces<List<InventoryTransactionBodyWeb>>(StatusCodes.Status200OK)
    .WithName("GetTransactionLog")
    .WithTags("Inventory");
//----
app.MapPost("/accuracy/WMS/api/v1/GetItem",
    [AllowAnonymous] async ([FromBody] InventoryRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<ItemResponseWeb> resp = poBL.SP_INVENTORY_WEB_GET_ITEM(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InventoryRequestWeb>("application/json")
    .Produces<List<ItemResponseWeb>>(StatusCodes.Status200OK)
    .WithName("GetItem")
    .WithTags("Inventory");
//------
app.MapPost("/accuracy/WMS/api/v1/GetInventoryItem",
    [AllowAnonymous] async ([FromBody] InventoryRequestWeb obj, HttpContext context) =>
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
            {  // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<InventoryResponseWeb> resp = poBL.SP_INVENTORY_WEB_GET_INVENTORY_ITEM(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización  
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de  
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autorizado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<InventoryRequestWeb>("application/json")
    .Produces<List<InventoryResponseWeb>>(StatusCodes.Status200OK)
    .WithName("GetInventoryItem")
    .WithTags("Inventory");
//----
app.MapPost("/accuracy/WMS/api/v1/GetInventoryWarehouse",
    [AllowAnonymous] async ([FromBody] WarehouseRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<InventoryResponseWeb> resp = poBL.SP_INVENTORY_WEB_GET_INVENTORY_WAREHOUSE(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<WarehouseRequestWeb>("application/json")
    .Produces<List<InventoryResponseWeb>>(StatusCodes.Status200OK)
    .WithName("GetInventoryWarehouse")
    .WithTags("Inventory");
//---
app.MapPost("/accuracy/WMS/api/v1/GetInventoryKardex",
    [AllowAnonymous] async ([FromBody] KardexRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<KardexBodyWeb> resp = poBL.SP_INVENTORY_WEB_GET_KARDEX(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<KardexRequestWeb>("application/json")
    .Produces<List<KardexBodyWeb>>(StatusCodes.Status200OK)
    .WithName("GetInventoryKardex")
    .WithTags("Inventory");
//---
app.MapPost("/accuracy/WMS/api/v1/GetInventoryCurva",
    [AllowAnonymous] async ([FromBody] CurvaRequestWeb obj, HttpContext context) =>
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
                AccuracyBussiness.InventoryBL.InventoryWebBL poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.InventoryBL.InventoryWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                List<CurvaResponsetWeb> resp = poBL.SP_INVENTORY_WEB_GET_CURVA(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
                };
                context.Response.StatusCode = 400;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<CurvaRequestWeb>("application/json")
    .Produces<List<CurvaResponsetWeb>>(StatusCodes.Status200OK)
    .WithName("GetInventoryCurva")
    .WithTags("Inventory");
#endregion
#region Forecast
app.MapPost("/accuracy/WMS/api/v1/GetForecastHead",
    [AllowAnonymous] async ([FromBody] ForecastHeadRequestWeb obj, HttpContext context) =>
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
            {  // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.ForecastBL.ForecastWebBL poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<ForecasHeadtBodyWeb> resp = poBL.SP_FORECAST_WEB_GET_HEAD(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<ForecastHeadRequestWeb>("application/json")
    .Produces<List<ForecasHeadtBodyWeb>>(StatusCodes.Status200OK)
    .WithName("GetForecastHead")
    .WithTags("Forecast");
//------
app.MapPost("/accuracy/WMS/api/v1/GetForecastDetail",
    [AllowAnonymous] async ([FromBody] ForecastDetailRequestWeb obj, HttpContext context) =>
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
            {  // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.ForecastBL.ForecastWebBL poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<ForecastDetailBodyWeb> resp = poBL.SP_FORECAST_WEB_GET_DETAIL(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
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
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<ForecastDetailRequestWeb>("application/json")
    .Produces<List<ForecastDetailBodyWeb>>(StatusCodes.Status200OK)
    .WithName("GetForecastDetail")
    .WithTags("Forecast");
//---
app.MapPost("/accuracy/WMS/api/v1/GetForecastDetailUpdate",
    [AllowAnonymous] async ([FromBody] ForecastDetailUpdateRequestWeb obj, HttpContext context) =>
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
            {   // Intenta validar el token
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // El token es válido, puedes continuar con la lógica de la ruta
                AccuracyBussiness.ForecastBL.ForecastWebBL poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                try
                {
                    poBL = new AccuracyBussiness.ForecastBL.ForecastWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ForecastDetailUpdateBodyWeb resp = poBL.SP_FORECAST_WEB_GET_DETAIL_UPDATE(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token invalido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token, devolver un error de autorización con mensaje JSON personalizado
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar token",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
        }
        else
        {
            // El usuario no está autenticado, devolvemos un error de autorización con mensaje JSON personalizado
            var errorResponse = new
            {
                title = "Warning",
                message = "usuario no autoeizado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<ForecastDetailUpdateRequestWeb>("application/json")
    .Produces<ForecastDetailUpdateBodyWeb>(StatusCodes.Status200OK)
    .WithName("GetForecastDetailUpdate")
    .WithTags("Forecast");

#endregion
#region Security
app.MapPost("/accuracy/WMS/api/v1/LoginWeb", [AllowAnonymous] async ([FromBody] UserRequest obj, HttpResponse response) =>
{
    AccuracyBussiness.SecurityBL.SecurityWebBL poBL = new AccuracyBussiness.SecurityBL.SecurityWebBL();
    string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
    List<UserResponse> resp = poBL.Login(obj, HostGroupId, connString);

    if (resp[0].mensaje == "EL USUARIO YA TIENE UNA SESIÓN ACTIVA EN OTRO DISPOSITIVO" || resp[0].mensaje == "LAS CREDENCIALES ESTAN INCORRECTAS" || resp[0].mensaje == "NO EXISTE EL USUARIO EN LA BASE DE DATOS, VERIFIQUE.")
    {
        var modalResponse = new
        {
            title = "Warning",
            message = resp[0].mensaje,
            type = resp[0].mensaje == "EL USUARIO YA TIENE UNA SESIÓN ACTIVA EN OTRO DISPOSITIVO" ? 1 : resp[0].mensaje == "LAS CREDENCIALES ESTAN INCORRECTAS" ? 2 : 3
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
        claims.Add(new Claim("almacen", "01"));
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
        mensaje = resp[0].mensaje
    }; 
    response.Headers.Add("Access-Control-Allow-Origin", "*");
    return Results.Ok(tokenResponse);
})
.Accepts<UserRequest>("application/json")
.Produces(StatusCodes.Status200OK)
.WithName("LoginWeb")
.WithTags("Security");

//-------------------   
app.MapPost("/accuracy/WMS/api/v1/GetWarehouseUser",
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
                        type = "0"
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
                        type = "0"
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
                    type = "0"

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
                    type = "0"
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
                title = "Error",
                message = "Usuario no autorizado",
                type = "0"
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
app.MapPost("/accuracy/WMS/api/v1/LogoutWeb",
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
#region KPI
app.MapPost("/accuracy/WMS/api/v1/GetOcupabilidad",
    [AllowAnonymous] async ([FromBody] OcupabilidadRequestWeb obj, HttpContext context) =>
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
                // El token es válido,   continuar con la lógica de la ruta 
                AccuracyBussiness.KPI.OcupabilidadWebBL poBL = new AccuracyBussiness.KPI.OcupabilidadWebBL();
                // Validar servidor y crear instancia de poBL
                try
                {
                    poBL = new AccuracyBussiness.KPI.OcupabilidadWebBL();
                }
                catch (Exception ex)
                {   // Ocurrió un error al crear la instancia de poBL o puede haber un problema de conexión
                    var errorResponse = new
                    {
                        title = "Error",
                        message = "Ocurrió un error al acceder al servidor",
                        type = "0"
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }
                string HostGroupId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                List<OcupabilidadBodyWeb> resp = poBL.SP_KPI_WEB_GET_OCUPABILIDAD(obj, HostGroupId, connString);
                context.Response.StatusCode = 200;
                return Results.Ok(resp);
            }
            catch (SecurityTokenValidationException)
            {
                // El token no es válido, devolver un error de autorización 
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Token inválido",
                    type = "0"
                };
                context.Response.StatusCode = 401;
                return Results.Json(errorResponse);
            }
            catch (Exception)
            {
                // Ocurrió un error al validar el token o cualquier otro error
                var errorResponse = new
                {
                    title = "Warning",
                    message = "Error al validar el token o procesar la solicitud",
                    type = "0"
                };
                context.Response.StatusCode = 400;
                return Results.Json(errorResponse);
            }
        }
        else
        {  // El usuario no está autenticado, devolvemos un error  
            var errorResponse = new
            {
                title = "Warning",
                message = "Usuario no autenticado",
                type = "0"
            };
            context.Response.StatusCode = 401;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<OcupabilidadRequestWeb>("application/json")
    .Produces<OcupabilidadRequestWeb>(StatusCodes.Status200OK)
    .WithName("GetOcupabilidad")
    .WithTags("KPI");


#endregion
#region PRINTER WEB
//
app.MapPost("/accuracy/WMS/api/v1/GetPrinter",
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
                        type = "0"
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
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
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
app.MapPost("/accuracy/WMS/api/v1/PostInsertLPNPrinter",
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
                        type = "0"
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
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
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
app.MapPost("/accuracy/WMS/api/v1/GetCorrelativeLPN",
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
                        type = "0"
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
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
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
//
app.MapPost("/accuracy/WMS/api/v1/PostInsertPackPrinter",
    [AllowAnonymous] async ([FromBody] RequestGenerabultos obj, HttpContext context) =>
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
                        type = 0
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                BodyGenerabultos resp = poBL.SP_PRINTER_WEB_POST_CONVERTED_PACK(obj, connString);
                if (resp.type == 0)
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return Results.Ok(resp);
                }
                else {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var errorResponse = new
                    {
                        title = resp.title,
                        message = resp.message,
                        type = resp.type
                    };
                    return Results.Json(errorResponse);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestGenerabultos>("application/json")
    .Produces<RequestGenerabultos>(StatusCodes.Status200OK)
    .WithName("PostInsertPackPrinter")
    .WithTags("Printer");
//
//
app.MapPost("/accuracy/WMS/api/v1/GetPack",
    [AllowAnonymous] async ([FromBody] RequestCabeceraBultos obj, HttpContext context) =>
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<BodyCabeceraBultos> resp = poBL.SP_PRINTER_WEB_GET_PACK(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestCabeceraBultos>("application/json")
    .Produces<RequestCabeceraBultos>(StatusCodes.Status200OK)
    .WithName("GetPack")
    .WithTags("Printer");
//
//
app.MapPost("/accuracy/WMS/api/v1/GetPackDetail",
    [AllowAnonymous] async ([FromBody] RequestDetalleBultos obj, HttpContext context) =>
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<BodyDetalleBultos> resp = poBL.SP_PRINTER_WEB_GET_PACK_DETAIL(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestDetalleBultos>("application/json")
    .Produces<RequestDetalleBultos>(StatusCodes.Status200OK)
    .WithName("GetPackDetail")
    .WithTags("Printer");
//
app.MapPost("/accuracy/WMS/api/v1/PostPrinterBultoxBultoxEan",
    [AllowAnonymous] async ([FromBody] RequestBultoxBulto obj, HttpContext context) =>
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<ResponseBultoxBulto> resp = poBL.SP_PRINTER_WEB_POST_INSERT_PRINTER_BULTO(obj, connString);
                context.Response.StatusCode = StatusCodes.Status200OK;
                return Results.Ok(resp);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    title = "Warning",
                    message = ex.Message.ToString(),
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<RequestBultoxBulto>("application/json")
    .Produces<RequestBultoxBulto>(StatusCodes.Status200OK)
    .WithName("PostPrinterBultoxBultoxEan")
    .WithTags("Printer");
//
#endregion
#region GENERAL
//
app.MapPost("/accuracy/WMS/api/v1/GetAttributeObject",
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
                        type = "0"
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
                    type = "0"
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
                type = "0"
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
app.MapPost("/accuracy/WMS/api/v1/PostCurva",
    [AllowAnonymous] async ([FromBody] CurvaRequest obj, HttpContext context) =>
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
                        type = "0"
                    };
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    return Results.Json(errorResponse);
                }

                List<CurvaRoot> resp = poBL.SP_GENERAL_WEB_POST_SEND_CURVAS_BY_OC(obj, connString);
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
                    type = "0"
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
                type = "0"
            };
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Results.Json(errorResponse);
        }
    })
    .Accepts<CurvaRequest>("application/json")
    .Produces<CurvaRequest>(StatusCodes.Status200OK)
    .WithName("PostCurva")
    .WithTags("General");
//
#endregion
app.Run();
