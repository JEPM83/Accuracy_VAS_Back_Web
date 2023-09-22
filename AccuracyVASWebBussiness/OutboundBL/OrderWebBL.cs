using AccuracyData.InboundDA;
using AccuracyData.InventoryDA;
using AccuracyData.OutboundDA;
using AccuracyData.SecurityDA;
using AccuracyModel.Inbound;
using AccuracyModel.Inventory;
using AccuracyModel.Outbound;
using AccuracyModel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.OutboundBL
{
    public class OrderWebBL
    {
        public List<OrderPickingBody> SP_OUTBOUND_WEB_GET_ORDER(OrderPickingRequest model, string HostGroupId, string cnx)
        {
            OrderWebDA poObjects = new OrderWebDA();
            List<OrderPickingBody> resp = new List<OrderPickingBody>();
            resp = poObjects.SP_OUTBOUND_WEB_GET_ORDER(model, cnx);
            return resp;
        }
        public List<Order_DetailBody> SP_OUTBOUND_WEB_GET_ORDER_DETAIL(Order_DetailRequest model, string HostGroupId, string cnx)
        {
            OrderWebDA poObjects = new OrderWebDA();
            List<Order_DetailBody> resp = new List<Order_DetailBody>();
            resp = poObjects.SP_OUTBOUND_WEB_GET_ORDER_DETAIL(model, cnx);
            return resp;
        }
        public List<OrderPickingBody> SP_OUTBOUND_WEB_POST_UPDATE_ORDER(OrderPickingUpdateRequest model, string HostGroupId, string cnx)
        {
            OrderWebDA poObjects = new OrderWebDA();
            List<OrderPickingBody> resp = new List<OrderPickingBody>();
            resp = poObjects.SP_OUTBOUND_WEB_POST_UPDATE_ORDER(model, cnx);
            return resp;
        }
        public List<OutboundTransactionBodyWeb> SP_OUTBOUND_WEB_GET_SHIPPING(OutboundTransactionRequestWeb model, string HostGroupId, string cnx)
        {
            OrderWebDA poObjects = new OrderWebDA();
            List<OutboundTransactionBodyWeb> resp = new List<OutboundTransactionBodyWeb>();
            resp = poObjects.SP_OUTBOUND_WEB_GET_SHIPPING(model, cnx);
            return resp;
        }

         
    }
}
