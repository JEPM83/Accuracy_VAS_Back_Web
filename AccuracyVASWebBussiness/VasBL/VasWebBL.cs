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
        public List<RootTaskObject> GET_ORDER_DETAIL_TASK_VAS(TaskRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<TaskResponse> objOriginal = new List<TaskResponse>();
            objOriginal = poObjects.GET_ORDER_DETAIL_TASK_VAS(model, cnx);
            /*Transformacion de objeto*/
            List<RootTaskObject> objTransformado = new List<RootTaskObject>();
                var groupedData = objOriginal.GroupBy(d => new { d.cliente, d.categoria_inventario }).Select(group => new RootTaskObject { 
                    cliente = group.Key.cliente,
                    categoria_inventario = group.Key.categoria_inventario,
                    grupo = group.GroupBy(d => new { d.id_agrupador,d.descripcion_agrupador}).Select(agrupadorGroup => new DetalleTaskGroup {
                    id_agrupador = agrupadorGroup.Key.id_agrupador,
                    descripcion_agrupador = agrupadorGroup.Key.descripcion_agrupador,
                    tareas = agrupadorGroup.Select(d => new SubDetalleTaskDetail { 
                        id_tarea = d.id_tarea,
                        secuencia = d.secuencia,
                        descripcion_tarea = d.descripcion_tarea
                    }).ToList()
                    }).ToList()
                }).ToList();
            /**/
            objTransformado = groupedData;
            return objTransformado;
        }
    }
}
