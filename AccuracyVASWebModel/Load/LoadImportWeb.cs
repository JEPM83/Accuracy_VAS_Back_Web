using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Load
{
    public class LoadImportRequest
    {
        public string id_almacen { get; set; }
        public string gui { get; set; }
        public string user { get; set; }
        public string proceso { get; set; }
        public string cliente { get; set; }

        public List<Dictionary<string, object>> data { get; set; }
    }
}
