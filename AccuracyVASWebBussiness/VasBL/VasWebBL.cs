using AccuracyData.SecurityDA;
using AccuracyData.VasDA;
using AccuracyModel.Security;
using AccuracyModel.Vas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.VasBL
{
    public class VasWebBL
    {
        public List<LineResponse> GET_PRODUCTION_LINE_BY_TERMINAL(LineRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<LineResponse> resp = new List<LineResponse>();
            resp = poObjects.GET_PRODUCTION_LINE_BY_TERMINAL(model, cnx);
            return resp;
        }
        public List<OrderPedidoResponse> GET_ORDER_VAS(OrderPedidoRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<OrderPedidoResponse> resp = new List<OrderPedidoResponse>();
            resp = poObjects.GET_ORDER_VAS(model, cnx);
            return resp;
        }
        public List<OrderPedidoDetailPickingResponse> GET_ORDER_DETAIL_PICKING_VAS(OrderPedidoDetailPickingRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<OrderPedidoDetailPickingResponse> resp = new List<OrderPedidoDetailPickingResponse>();
            resp = poObjects.GET_ORDER_DETAIL_PICKING_VAS(model, cnx);
            return resp;
        }
    }
}
