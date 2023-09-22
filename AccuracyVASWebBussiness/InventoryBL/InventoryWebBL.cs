using AccuracyData.InventoryDA;
using AccuracyModel.Inventory;
using AccuracyModel.Outbound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.InventoryBL
{
    public class InventoryWebBL
    {
        public List<InventoryTransactionBodyWeb> SP_INVENTORY_WEB_GET_TRANSACTION_LOG(InventoryTransactionRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<InventoryTransactionBodyWeb> resp = new List<InventoryTransactionBodyWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_TRANSACTION_LOG(model, cnx);
            return resp;
        }
        public List<ItemResponseWeb> SP_INVENTORY_WEB_GET_ITEM(InventoryRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<ItemResponseWeb> resp = new List<ItemResponseWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_ITEM(model, cnx);
            return resp;
        }
        public List<InventoryResponseWeb> SP_INVENTORY_WEB_GET_INVENTORY_ITEM(InventoryRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<InventoryResponseWeb> resp = new List<InventoryResponseWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_INVENTORY_ITEM(model, cnx);
            return resp;
        }
        public List<InventoryResponseWeb> SP_INVENTORY_WEB_GET_INVENTORY_WAREHOUSE(WarehouseRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<InventoryResponseWeb> resp = new List<InventoryResponseWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_INVENTORY_WAREHOUSE(model, cnx);
            return resp;
        }
        public List<KardexBodyWeb> SP_INVENTORY_WEB_GET_KARDEX(KardexRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<KardexBodyWeb> resp = new List<KardexBodyWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_KARDEX(model, cnx);
            return resp;
        }
        public List<CurvaResponsetWeb> SP_INVENTORY_WEB_GET_CURVA(CurvaRequestWeb model, string HostGroupId, string cnx)
        {
            InventoryWebDA poObjects = new InventoryWebDA();
            List<CurvaResponsetWeb> resp = new List<CurvaResponsetWeb>();
            resp = poObjects.SP_INVENTORY_WEB_GET_CURVA(model, cnx);
            return resp;
        }
    }
}
