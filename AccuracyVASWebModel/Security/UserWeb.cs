using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Security
{
    public class UserRequest
    {
        public string usuario { get; set; }
        public string? password { get; set; }
        public string? sistema { get; set; }
        public string? identificador { get; set; }
    }
    public class UserResponse
    {
        public string usuario { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string estado_sesion { get; set; }
        public string guid_sesion { get; set; }
        public string status { get; set; }
        public string mensaje { get; set; }
        public string? linea_produccion { get; set; }
        public string? ruta { get; set; }
    }
    public class UserWarehouseRequest {
        public string usuario { get; set; }
    }
    public class UserWarehouseResponse
    {
        public string id_almacen { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string codigo_pais { get; set; }
    }
}