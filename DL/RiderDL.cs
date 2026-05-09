using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public class RiderDL
    {
        public static void AddRider(RiderBL r)
        {
            string query = "INSERT INTO riders (user_id,vehicle_type) VALUES (@UserId,@Vehicle)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@UserId", r.user_id),
                new MySqlParameter("@Vehicle", r.vehicle_type));
        }
    }

}
