using AccuracyData.InboundDA;
using AccuracyData.OutboundDA;
using AccuracyModel.Inbound;
using AccuracyModel.Outbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.InboundBL
{
    public class PurchaseOrderWebBL
    {
        public List<PurchaseOrderBodyWeb> SP_INBOUND_WEB_GET_ORDER(PurchaseOrderRequestWeb model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<PurchaseOrderBodyWeb> resp = new List<PurchaseOrderBodyWeb>();
            resp = poObjects.SP_INBOUND_WEB_GET_ORDER(model, cnx);
            return resp;
        }
        public List<PurchaseOrderDetailBodyWeb> SP_OUTBOUND_WEB_GET_ORDER_DETAIL(PurchaseOrderDetailRequestWeb model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<PurchaseOrderDetailBodyWeb> resp = new List<PurchaseOrderDetailBodyWeb>();
            resp = poObjects.SP_INBOUND_WEB_GET_ORDER_DETAIL(model, cnx);
            return resp;
        }
        public List<InboundTransactionBodyWeb> SP_INBOUND_WEB_GET_RECEIPT(InboundTransactionRequestWeb model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<InboundTransactionBodyWeb> resp = new List<InboundTransactionBodyWeb>();
            resp = poObjects.SP_INBOUND_WEB_GET_RECEIPT(model, cnx);
            return resp;
        }
        public List<DetalleRecepResponse> SP_INBOUND_WEB_GET_RECEIPT(DetalleRecepRequest model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<DetalleRecepResponse> resp = new List<DetalleRecepResponse>();
            resp = poObjects.SP_INBOUND_WEB_GET_RECEIPT(model, cnx);
            return resp;
        }
        public List<DetalleRecep_IDResponse> SP_INBOUND_WEB_GET_RECEIPT_LINE(DetalleRecep_IDRequest model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<DetalleRecep_IDResponse> resp = new List<DetalleRecep_IDResponse>();
            resp = poObjects.SP_INBOUND_WEB_GET_RECEIPT_LINE(model, cnx);
            return resp;
        }
        public CloseReceiptResponse SP_INBOUND_WEB_POST_SYNC_PURCHASE_ORDER(CloseReceipRequest model, string HostGroupId, string cnx) {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            CloseReceiptResponse resp = new CloseReceiptResponse();
            resp = poObjects.SP_INBOUND_WEB_POST_SYNC_PURCHASE_ORDER(model, cnx);
            return resp;
        }
        public InsertReceiptResponse SP_INBOUND_WEB_POST_INSERT_PURCHASE_ORDER(InsertReceiptRequest model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            InsertReceiptResponse resp = new InsertReceiptResponse();
            resp = poObjects.SP_INBOUND_WEB_POST_INSERT_PURCHASE_ORDER(model, cnx);
            return resp;
        }
        public List<PurchaseOrderEanResponse> SP_OUTBOUND_WEB_GET_ORDER_DETAIL_EAN(PurchaseOrderEanRequest model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<PurchaseOrderEanResponse> resp = new List<PurchaseOrderEanResponse>();
            resp = poObjects.SP_INBOUND_WEB_GET_ORDER_DETAIL_EAN(model, cnx);
            return resp;
        }
        public List<ReceiptLineEanResponse> SP_INBOUND_WEB_GET_RECEIPT_DETAIL_EAN(ReceiptLineEanRequest model, string HostGroupId, string cnx)
        {
            PurchaseOrderWebDA poObjects = new PurchaseOrderWebDA();
            List<ReceiptLineEanResponse> resp = new List<ReceiptLineEanResponse>();
            resp = poObjects.SP_INBOUND_WEB_GET_RECEIPT_DETAIL_EAN(model, cnx);
            return resp;
        }
    }
}

