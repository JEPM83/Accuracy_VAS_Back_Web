using AccuracyData.SecurityDA;
using AccuracyModel.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuracyBussiness.SecurityBL
{
    public class SecurityWebBL
    {
        public List<UserResponse> Login(UserRequest model, string HostGroupId, string cnx)
        {
            SecurityWebDA poObjects = new SecurityWebDA();
            List<UserResponse> resp = new List<UserResponse>();
            resp = poObjects.Login(model, cnx);
            return resp;
        }
        public List<UserResponse> Logout(UserRequest model, string HostGroupId, string cnx)
        {
            SecurityWebDA poObjects = new SecurityWebDA();
            List<UserResponse> resp = new List<UserResponse>();
            resp = poObjects.Logout(model, cnx);
            return resp;
        }
        public List<UserWarehouseResponse> WarehouseUser(UserWarehouseRequest model, string HostGroupId, string cnx)
        {
            SecurityWebDA poObjects = new SecurityWebDA();
            List<UserWarehouseResponse> resp = new List<UserWarehouseResponse>();
            resp = poObjects.WarehouseUser(model, cnx);
            return resp;
        }
    }
}
