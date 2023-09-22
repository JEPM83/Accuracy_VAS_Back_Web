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
        public static string WEB_GET_DATALISTLOGIN = "SP_SECURITY_WEB_LOGIN";
        public static string WEB_GET_DATALISTLOGOUT = "SP_SECURITY_WEB_LOGOUT";
        public static string WEB_GET_WAREHOUSE_USER = "SP_SECURITY_WEB_WAREHOUSE_USER";
        #endregion
        #region InboundWeb
        public static string WEB_GET_INBOUND_ORDER = "SP_INBOUND_WEB_GET_ORDER";
        public static string WEB_GET_INBOUND_ORDER_DETAIL = "SP_INBOUND_WEB_GET_ORDER_DETAIL";
        public static string WEB_GET_INBOUND_RECEIPT = "SP_OUTBOUND_WEB_GET_RECEIPT"; //PAMETECH
        public static string WEB_GET_DETALLE_RECEPCION = "SP_INBOUND_WEB_GET_RECEIPT";
        public static string WEB_GET_DETALLE_RECEPCION_LINEA = "SP_INBOUND_WEB_GET_RECEIPT_LINE";
        public static string WEB_POST_CLOSE_RECEIPT = "SP_INBOUND_HH_POST_SYNC_PURCHASE_ORDER"; //EVALUAR SI SE ESTANDARIZAR EL NOMBRE YA QUE EL MISMO PROCESO USA EL APP
        public static string WEB_POST_INSERT_PURCHASE_ORDER = "SP_INBOUND_WEB_POST_INSERT_RECEIPT_PURCHASE_ORDER";
        public static string WEB_GET_INBOUND_ORDER_DETAIL_EAN = "SP_INBOUND_WEB_GET_ORDER_DETAIL_EAN";
        public static string WEB_GET_INBOUND_RECEIPT_DETAIL_EAN = "SP_INBOUND_WEB_GET_RECEIPT_LINE_EAN";
        #endregion
        #region OutboundWeb
        public static string WEB_GET_OUTBOUND_ORDER = "SP_OUTBOUND_WEB_GET_ORDER";
        public static string WEB_GET_OUTBOUND_ORDER_DETAIL = "SP_OUTBOUND_WEB_GET_ORDER_DETAIL";
        public static string WEB_POST_OUTBOUND_UPDATE_ORDER = "SP_OUTBOUND_WEB_POST_UPDATE_ORDER";
        public static string WEB_GET_OUTBOUND_SHIPPING = "SP_OUTBOUND_WEB_GET_SHIPPING";
  
        #endregion
        #region InventoryWeb
        public static string WEB_GET_TRANSACTION_LOG = "SP_INVENTORY_WEB_GET_TRANSACTION_LOG";
        public static string WEB_GET_ITEM = "SP_INVENTORY_WEB_GET_ITEM";
        public static string WEB_GET_INVENTORY_ITEM = "SP_INVENTORY_WEB_GET_INVENTORY_ITEM";
        public static string WEB_GET_INVENTORY_WAREHOUSE = "SP_INVENTORY_WEB_GET_INVENTORY_WAREHOUSE";
        public static string WEB_GET_INVENTORY_KARDEX = "SP_INVENTORY_WEB_GET_KARDEX";
        public static string WEB_GET_INVENTORY_CURVA = "SP_INVENTORY_WEB_GET_CURVA";
        #endregion
        #region LoadImport
        public static string WEB_GET_LOAD_OBJECT_IMPORT = "SP_LOAD_WEB_POST_INSERT_OBJECT";
        #endregion
        #region Forecast
        public static string WEB_GET_FORECAST_HEAD = "SP_FORECAST_WEB_GET_HEAD";
        public static string WEB_GET_FORECAST_DETAIL = "SP_FORECAST_WEB_GET_DETAIL";
        public static string WEB_GET_FORECAST_DETAIL_UPDATE = "SP_FORECAST_WEB_POST_UPDATE_QUANTITY_DETAIL";
        #endregion
        #region KPI
        public static string WEB_GET_KPI_OCUPABILIDAD = "SP_KPI_WEB_GET_OCUPABILIDAD";
        #endregion
        #region PRINTER
        public static string WEB_GET_PRINTER_CONFIG = "SP_PRINTER_WEB_GET_PRINTER_CONFIG";
        public static string WEB_POST_INSERT_PRINTER_LPN = "SP_PRINTER_WEB_POST_INSERT_PRINTER_LPN";
        public static string WEB_GET_LPN_CORRELATIVE = "SP_PRINTER_WEB_GET_LPN_CORRELATIVE";
        public static string WEB_POST_CONVERTED_PACK = "SP_PRINTER_WEB_POST_CONVERTED_PACK"; //ITSANET
        public static string WEB_GET_PACK = "SP_PRINTER_WEB_GET_PACK";  //ITSANET
        public static string WEB_GET_PACK_DETAIL = "SP_PRINTER_WEB_GET_PACK_DETAIL";  //ITSANET
        public static string WEB_POST_INSERT_PRINTER_BULTO = "SP_PRINTER_WEB_POST_INSERT_PRINTER_BULTO"; //ITSANET
        #endregion
        #region GENERAL
        public static string WEB_GET_LISTA_ATRIBUTO_OBJETO= "SP_GENERAL_WEB_GET_LISTA_ATRIBUTO_OBJETO";
        public static string WEB_POST_SEND_CURVAS_BY_OC = "SP_GENERAL_WEB_POST_SEND_CURVAS_BY_OC";
        #endregion
    }
}
