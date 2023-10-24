﻿using AccuracyData.SecurityDA;
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
            List<OrderPedidoResponse> resp1 = new List<OrderPedidoResponse>();
            int contador = 0;
            string pedido = string.Empty;
            string categoria_inventario = string.Empty;
            foreach (var a in resp)
            {
                if (contador == 0)
                {
                    pedido = a.numero_pedido;
                    categoria_inventario = a.categoria_inventario;
                }
                if (a.numero_pedido == pedido && contador > 0)
                {
                    categoria_inventario = String.Concat(categoria_inventario, '/', a.categoria_inventario);
                    resp1[contador - 1].categoria_inventario = categoria_inventario;
                }
                else
                {
                    OrderPedidoResponse b = new OrderPedidoResponse();
                    categoria_inventario = a.categoria_inventario;
                    b.cliente = a.cliente;
                    b.tienda = a.tienda;
                    b.numero_pedido = a.numero_pedido;
                    b.avance_vas = a.avance_vas;
                    b.lineas_produccion = a.lineas_produccion;
                    b.id_hu = a.id_hu;
                    b.categoria_inventario = categoria_inventario;
                    b.color_fondo = a.color_fondo;
                    b.medida = a.medida;
                    resp1.Add(a);
                    pedido = a.numero_pedido;
                    contador++;
                }
            }
            return resp1;
        }
        public List<NotifyOrderResponse> POST_NOTIFY_ORDER_VAS(NotifyOrderRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<NotifyOrderResponse> resp = new List<NotifyOrderResponse>();
            resp = poObjects.POST_NOTIFY_ORDER_VAS(model, cnx);
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
        public List<InicioTareaResponse> POST_START_TASK_VAS(InicioTareaRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<InicioTareaResponse> resp = new List<InicioTareaResponse>();
            resp = poObjects.POST_START_TASK_VAS(model, cnx);
            return resp;
        }
        public List<PackingdetailResponse> GET_HU_DETAIL_BY_ORDER_VAS(PackingdetailRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<PackingdetailResponse> resp = new List<PackingdetailResponse>();
            resp = poObjects.GET_HU_DETAIL_BY_ORDER_VAS(model, cnx);
            return resp;
        }
        public List<LPNvalidaResponse> GET_LPN_VALIDATE_VAS(LPNvalidateRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<LPNvalidaResponse> resp = new List<LPNvalidaResponse>();
            resp = poObjects.GET_LPN_VALIDATE_VAS(model, cnx);
            return resp;
        }
        public List<LPNSKUResponse> POST_LPN_SKU_VAS(LPNSKURequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<LPNSKUResponse> resp = new List<LPNSKUResponse>();
            resp = poObjects.POST_LPN_SKU_VAS(model, cnx);
            return resp;
        }
        public List<DeleteVasResponse> POST_DELETE_VAS(DeleteVasRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<DeleteVasResponse> resp = new List<DeleteVasResponse>();
            resp = poObjects.POST_DELETE_VAS(model, cnx);
            return resp;
        }
        public List<FinTareaResponse> POST_END_TASK_VAS(FinTareaRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<FinTareaResponse> resp = new List<FinTareaResponse>();
            resp = poObjects.POST_END_TASK_VAS(model, cnx);
            return resp;
        }
        public List<InicioIncidenciaResponse> POST_START_INCIDENCE_VAS(InicioIncidenciaRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<InicioIncidenciaResponse> resp = new List<InicioIncidenciaResponse>();
            resp = poObjects.POST_START_INCIDENCE_VAS(model, cnx);
            return resp;
        }
        public List<FinIncidenciaResponse> POST_END_INCIDENCE_VAS(FinIncidenciaRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<FinIncidenciaResponse> resp = new List<FinIncidenciaResponse>();
            resp = poObjects.POST_END_INCIDENCE_VAS(model, cnx);
            return resp;
        }
        public List<ListaNotificacionResponse> GET_LIST_NOTIFY_VAS(ListaNotificacionRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<ListaNotificacionResponse> resp = new List<ListaNotificacionResponse>();
            resp = poObjects.GET_LIST_NOTIFY_VAS(model, cnx);
            return resp;
        }
        public List<ActualizacionNotificacionResponse> POST_UPDATE_NOTIFY_VAS(ActualizacionNotificacionRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<ActualizacionNotificacionResponse> resp = new List<ActualizacionNotificacionResponse>();
            resp = poObjects.POST_UPDATE_NOTIFY_VAS(model, cnx);
            return resp;
        }
        public List<PanelLineaResponse> GET_PANEL_LINEA_PRODUCCION_VAS(PanelLineaRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<PanelLineaResponse> resp = new List<PanelLineaResponse>();
            resp = poObjects.GET_PANEL_LINEA_PRODUCCION_VAS(model, cnx);
            return resp;
        }
        public List<PanelOrdenResponse> GET_PANEL_ORDER_PRODUCCION_VAS(PanelOrdenRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<PanelOrdenResponse> resp = new List<PanelOrdenResponse>();
            resp = poObjects.GET_PANEL_ORDER_PRODUCCION_VAS(model, cnx);
            return resp;
        }
        public List<OrdenesVasResponse> GET_LIST_ORDER_PRODUCCION_VAS(OrdenesVasRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<OrdenesVasResponse> resp = new List<OrdenesVasResponse>();
            resp = poObjects.GET_LIST_ORDER_PRODUCCION_VAS(model, cnx);
            return resp;
        }
        public List<UsuariosVasResponse> GET_LIST_STATE_USERS_VAS(UsuariosVasRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<UsuariosVasResponse> resp = new List<UsuariosVasResponse>();
            resp = poObjects.GET_LIST_STATE_USERS_VAS(model, cnx);
            return resp;
        }
        public List<B2BVasResponse> GET_LIST_ORDER_B2B_VAS(B2BVasRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<B2BVasResponse> resp = new List<B2BVasResponse>();
            resp = poObjects.GET_LIST_ORDER_B2B_VAS(model, cnx);
            return resp;
        }
        public List<PrinterLpnResponse> POST_PRINTER_VAS(PrinteLpnRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<PrinterLpnResponse> resp = new List<PrinterLpnResponse>();
            resp = poObjects.POST_PRINTER_VAS(model, cnx);
            return resp;
        }
    }
}
