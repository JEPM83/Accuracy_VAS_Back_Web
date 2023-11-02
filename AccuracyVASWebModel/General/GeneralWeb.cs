using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccuracyModel.General
{
    public class GeneralObjetoRequestWeb
    {
        public string objeto { get; set; }
        public string tipo { get; set; }
        public string idioma { get; set; }
        public bool modo { get; set; }
    }
    public class GeneralObjetotBodyWeb
    {
        public string value { get; set; }
        public string display { get; set; }
        public string display_large { get; set; }
    }
    public class ClientRequest
    {
        public string id_almacen { get; set; }
    }
    public class ClientResponse
    {
        public string valuemember { get; set; }
        public string displaymember { get; set; }
    }
}
