using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyModel.Inventory
{
    public class InventoryTransactionRequestWeb
    {
        public string id_almacen { get; set; }
        public string numero_control { get; set; }
        public string id_ubicacion { get; set; }
        public string id_empleado { get; set; }
        public string fecha_inicial { get; set; }
        public string? fecha_final { get; set; }
        public string? numero_item { get; set; }
        public string? tipo_transaccion { get; set; }
    }
    public class InventoryTransactionBodyWeb
    {
        public string id { get; set; }
        public string tipo_transaccion { get; set; }
        public string inicio_fecha { get; set; }
        public string fin_fecha { get; set; }
        public string id_empleado { get; set; }
        public string numero_control { get; set; }
        public string id_almacen { get; set; }
        public string id_ubicacion { get; set; }
        public string numero_item { get; set; }
        public string codigo_item { get; set; }
        public string? numero_lote { get; set; }
        public string? fecha_expiracion { get; set; }
        public string talla { get; set; }
        public string id_hu { get; set; }
        public decimal cantidad_transaccion { get; set; }
        public string tipo_mov { get; set; }
        public string atributo_generico_1 { get; set; }
        public string atributo_generico_2 { get; set; }
        public string nombre_item { get; set; }
        public string? familia { get; set; }
        public string? sub_familia { get; set; }
        public string? id_hu_original { get; set; }

    }
    public class InventoryRequestWeb
    {
        public string id_almacen { get; set; }
        public string? numero_item { get; set; }
    }
    public class ItemResponseWeb
    {
        public string id_almacen { get; set; }
        public string numero_item { get; set; }
        public string descripcion { get; set; }
        public string tipo_inventario { get; set; }
        public string unidad_medida { get; set; }
        public string? id_clase { get; set; }
        public string? upc { get; set; }
        public string? uom { get; set; }
    }
    public class WarehouseRequestWeb
    {
        public string id_almacen { get; set; }
    }
    public class InventoryResponseWeb
    {
        public string numero_item { get; set; }

        public string nombre_item { get; set; }
        public string? numero_lote { get; set; }
        public string? fecha_expiracion { get; set; }
        public string id_almacen { get; set; }
        public string id_ubicacion { get; set; }
        public string? id_hu { get; set; }
        public float cantidad_actual { get; set; }
        public string? fecha_fifo { get; set; }
        public float cantidad_nodisponible { get; set; }
        public string? atributo_generico_1 { get; set; }
        public string? atributo_generico_2 { get; set; }
    }
    public class KardexRequestWeb
    {
        public string id_almacen { get; set; }
        public string? fecha_inicial { get; set; }
        public string? fecha_final { get; set; }
        public string? numero_item { get; set; }
    }
    public class KardexBodyWeb
    {
        public float TotalForecasting { get; set; }
        public string? Articulo { get; set; }
        public string? fecha_final { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Movimiento { get; set; }
        public string? Guia { get; set; }
        public string? Documento { get; set; }
        public string? Serie { get; set; }
        public float? Numero { get; set; }
        public string? Cliente_Proveedor { get; set; }
        public string? Observacion { get; set; }
        public string? Ubicacion { get; set; }
        public float? CantidadE { get; set; }
        public float? PrecioE { get; set; }
        public float? TotalE { get; set; }
        public float? CantidadS { get; set; }
        public float? PrecioS { get; set; }
        public float? TotalS { get; set; }
    }
    public class CurvaRequestWeb {
        public string id_almacen { get; set; }
        public string numero_item { get; set; }
        public string? curva { get; set; }
    }
    public class CurvaResponsetWeb
    {
        public string curva { get; set; }
        public string talla { get; set; }
        public int cantidad { get; set; }
    }
}
