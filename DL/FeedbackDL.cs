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
    public class FeedbackDL
    {
        public static void AddFeedback(FeedbackBL f)
        {
            string query = "INSERT INTO feedback (user_id,restaurant_id,order_id,rating,comment) VALUES (@UserId,@RestaurantId,@OrderId,@Rating,@Comment)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@UserId", f.user_id),
                new MySqlParameter("@RestaurantId", f.restaurant_id),
                new MySqlParameter("@OrderId", f.order_id),
                new MySqlParameter("@Rating", f.rating),
                new MySqlParameter("@Comment", f.comment));
        }
    }

}
