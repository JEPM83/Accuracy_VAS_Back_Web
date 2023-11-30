using AccuracyData.SecurityDA;
using AccuracyData.VasDA;
using AccuracyModel.Security;
using AccuracyModel.Vas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
        public RootB2BResponse GET_LIST_ORDER_B2B_VAS_V2(SendB2BVas_baseRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<SendB2BVas_atributos> resp = new List<SendB2BVas_atributos>();
            resp = poObjects.GET_LIST_ORDER_B2B_VAS_V2(model, cnx);
            int i = 0;           
            /*Transformacion de objeto*/
            RootB2BResponse objTransformado = new RootB2BResponse();
            RootB2BResponse objRootB2BResponse = null;
            foreach (var item in resp)
            {
                if (objRootB2BResponse == null)
                {
                    objRootB2BResponse = new RootB2BResponse
                    {
                        cabeceraB2BResponse = new CabeceraB2BResponse(), // Asignación por defecto
                        cuerpoB2BResponse = new List<CuerpoB2BResponse>(),
                        pieB2BResponse = new PieB2BResponse() // Asignación por defecto
                    };
                }
                if (item is SendB2BVas_Modelo1Detalle detalle1 && (model.id_destino == "D003" || model.id_destino == "D004") && i == 0)
                {
                    var lista = new CabeceraB2BResponse()
                    {
                        numero_de_cita = detalle1.numero_de_cita,
                        numero_de_oc = detalle1.numero_de_oc,
                        ruc = detalle1.ruc,
                        fecha = detalle1.fecha,
                        documento = detalle1.documento,
                        contenedor = detalle1.contenedor
                    };
                    objRootB2BResponse.cabeceraB2BResponse = lista;
                }
                else if (item is SendB2BVas_Modelo2Detalle detalle2 && (model.id_destino == "D001" || model.id_destino == "D002") && i == 0)
                {
                    var lista = new CabeceraB2BResponse()
                    {
                        nro_lote = detalle2.nro_lote
                    };
                    objRootB2BResponse.cabeceraB2BResponse = lista;
                }
                else
                {
                    // Handle other cases or set cabeceraB2BResponse to null if needed
                }
                if (item is SendB2BVas_Modelo1Detalle cuerpo1 && (model.id_destino == "D003" || model.id_destino == "D004"))
                {
                    var lista = new CuerpoB2BResponse();
                    lista.correlativo = cuerpo1.correlativo;
                    lista.articulo = cuerpo1.articulo;
                    lista.cantidad = cuerpo1.cantidad;
                    lista.na = cuerpo1.na;
                    lista.cod_sucursal = cuerpo1.cod_sucursal;
                    lista.costo_unitario = cuerpo1.costo_unitario;
                    objRootB2BResponse.cuerpoB2BResponse.Add(lista);
                }
                else if (item is SendB2BVas_Modelo2Detalle cuerpo2 && (model.id_destino == "D001" || model.id_destino == "D002"))
                {
                    var lista = new CuerpoB2BResponse();
                    lista.negocio = cuerpo2.negocio;
                    lista.orden_de_compra = cuerpo2.orden_de_compra;
                    lista.upc_ean = cuerpo2.upc_ean;
                    lista.descripcion = cuerpo2.descripcion;
                    lista.caducidad = cuerpo2.caducidad;
                    lista.cantidad = cuerpo2.cantidad;
                    lista.cantidad_a_enviar = cuerpo2.cantidad_a_enviar;
                    lista.tienda_destino = cuerpo2.tienda_destino;
                    lista.nombre_tienda = cuerpo2.nombre_tienda;
                    lista.tipo_embalaje = cuerpo2.tipo_embalaje;
                    lista.lpn = cuerpo2.lpn;
                    objRootB2BResponse.cuerpoB2BResponse.Add(lista);
                }
                else if (item is SendB2BVas_Modelo3Detalle cuerpo3 && (model.id_destino == "D005" || model.id_destino == "D006"))
                {
                    var lista = new CuerpoB2BResponse();
                    lista.codigo_proveedor = cuerpo3.codigo_proveedor;
                    lista.sucursal_destino = cuerpo3.sucursal_destino;
                    lista.n_de_oc = cuerpo3.n_de_oc;
                    lista.lpn = cuerpo3.lpn;
                    lista.codigo_producto = cuerpo3.codigo_producto;
                    lista.cantidad_und = cuerpo3.cantidad_und;
                    objRootB2BResponse.cuerpoB2BResponse.Add(lista);
                }
                else if (item is SendB2BVas_Modelo4Detalle cuerpo4)
                {
                    var lista = new CuerpoB2BResponse();
                    lista.o_r = cuerpo4.o_r;
                    lista.o_c = cuerpo4.o_c;
                    lista.id_destino = cuerpo4.id_destino;
                    lista.nombre_destino = cuerpo4.nombre_destino;
                    lista.destino_cod = cuerpo4.destino_cod;
                    lista.destino_des = cuerpo4.destino_des;
                    lista.sku = cuerpo4.sku;
                    lista.item = cuerpo4.item;
                    lista.talla = cuerpo4.talla;
                    lista.cantidad = cuerpo4.cantidad;
                    lista.ean13 = cuerpo4.ean13;
                    lista.precio = cuerpo4.precio;
                    lista.lpn = cuerpo4.lpn;
                    objRootB2BResponse.cuerpoB2BResponse.Add(lista);
                }
                if (i == 0)
                {
                    var lista = new PieB2BResponse()
                    {
                        columna = item.columna,
                        fila = item.fila,
                        salto = item.salto,
                        titulo = item.titulo,
                        nombre_archivo = item.nombre_archivo,
                        extension_archivo = item.extension_archivo,
                        hoja = item.hoja,
                        color_fondo_titulo_grilla = item.color_fondo_titulo_grilla,
                        color_letra_titulo_grilla = item.color_letra_titulo_grilla
                    };
                    objRootB2BResponse.pieB2BResponse = lista;
                }

                objTransformado = objRootB2BResponse;
                i++;
            }
            var filteredResponse = new RootB2BResponse
            {
                cabeceraB2BResponse = FilterNullProperties(objTransformado.cabeceraB2BResponse),
                cuerpoB2BResponse = objTransformado.cuerpoB2BResponse?.Where(c => !AllPropertiesNull(c)).ToList(),
                pieB2BResponse = FilterNullProperties(objTransformado.pieB2BResponse)
            };
            return filteredResponse;
        }
        private CabeceraB2BResponse FilterNullProperties(CabeceraB2BResponse input)
        {
            return new CabeceraB2BResponse
            {
                numero_de_cita = input?.numero_de_cita,
                numero_de_oc = input?.numero_de_oc,
                ruc = input?.ruc,
                fecha = input?.fecha,
                documento = input?.documento,
                contenedor = input?.contenedor,
                nro_lote = input?.nro_lote
            };
        }
        private PieB2BResponse FilterNullProperties(PieB2BResponse input)
        {
            return new PieB2BResponse
            {
                columna = (int)(input?.columna),
                fila = (int)(input?.fila),
                salto = input?.salto,
                titulo = input?.titulo,
                nombre_archivo = input?.nombre_archivo,
                extension_archivo = input?.extension_archivo,
                hoja = input?.hoja,
                color_fondo_titulo_grilla = input?.color_fondo_titulo_grilla,
                color_letra_titulo_grilla = input?.color_letra_titulo_grilla
            };
        }
        private bool AllPropertiesNull(CuerpoB2BResponse input)
        {
            return input == null ||
                   input.GetType().GetProperties().All(p => p.GetValue(input) == null);
        }
        public List<PrinterLpnResponse> POST_PRINTER_VAS(PrinteLpnRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<PrinterLpnResponse> resp = new List<PrinterLpnResponse>();
            resp = poObjects.POST_PRINTER_VAS(model, cnx);
            return resp;
        }
        public List<UpdateStateResponse> POST_UPDATE_STATE_DELIVERY_VAS(UpdateStateRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<UpdateStateResponse> resp = new List<UpdateStateResponse>();
            resp = poObjects.POST_UPDATE_STATE_DELIVERY_VAS(model, cnx);
            return resp;
        }
        public List<DestinoResponse> GET_LIST_DESTINITY(DestinoRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<DestinoResponse> resp = new List<DestinoResponse>();
            resp = poObjects.GET_LIST_DESTINITY(model, cnx);
            return resp;
        }
        public List<ImprimirLpnResponse> POST_PRINTER_LPN_VAS(ImprimirLpnRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<ImprimirLpnResponse> resp = new List<ImprimirLpnResponse>();
            resp = poObjects.POST_PRINTER_LPN_VAS(model, cnx);
            return resp;
        }
        public List<CorrelativoLpnResponse> GET_LPN_CORRELATIVE_DESTINITY(CorrelativoLpnRequest model, string cnx)
        {
            VasWebDA poObjects = new VasWebDA();
            List<CorrelativoLpnResponse> resp = new List<CorrelativoLpnResponse>();
            resp = poObjects.GET_LPN_CORRELATIVE_DESTINITY(model, cnx);
            return resp;
        }
    }
}
