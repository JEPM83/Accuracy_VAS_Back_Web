using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyData.ObjectosDA
{
    static class ObjectsDA
    {
        #region Autenticacion
        public static string SECURITY_GET_DATALISTLOGIN = "SP_SECURITY_WEB_LOGIN";
        public static string SECURITY_GET_DATALISTLOGOUT = "SP_SECURITY_WEB_LOGOUT";
        public static string SECURITY_GET_WAREHOUSE_USER = "SP_SECURITY_WEB_WAREHOUSE_USER";
        #endregion
        #region PRINTER
        public static string PRINTER_GET_PRINTER_CONFIG = "SP_PRINTER_WEB_GET_PRINTER_CONFIG";
        public static string PRINTER_POST_INSERT_PRINTER_LPN = "SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN";
        public static string PRINTER_GET_LPN_CORRELATIVE = "SP_PRINTER_WEB_GET_LPN_CORRELATIVE";
        #endregion
        #region GENERAL
        public static string GENERAL_GET_LISTA_ATRIBUTO_OBJETO= "SP_GENERAL_WEB_GET_LISTA_ATRIBUTO_OBJETO";
        #endregion
        #region VAS
        public static string VAS_GET_PRODUCTION_LINE_BY_TERMINAL = "SP_VAS_WEB_GET_PRODUCTION_LINE_BY_TERMINAL";
        public static string VAS_GET_ORDER_VAS = "SP_VAS_WEB_GET_ORDER_VAS";
        public static string VAS_GET_ORDER_DETAIL_PICKING_VAS = "SP_VAS_WEB_GET_ORDER_DETAIL_PICKING_VAS";
        public static string VAS_GET_ORDER_DETAIL_TASK_VAS = "SP_VAS_WEB_GET_ORDER_DETAIL_TASK_VAS";
        #endregion
    }
}
