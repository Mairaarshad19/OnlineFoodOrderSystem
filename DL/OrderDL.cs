using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public static class OrderDL
    {
        public static List<OrderBL> GetOrdersByRestaurant(int restaurantId)
        {
            string query = "SELECT * FROM orders WHERE restaurant_id=@RestaurantId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySql.Data.MySqlClient.MySqlParameter("@RestaurantId", restaurantId));

            List<OrderBL> orders = new List<OrderBL>();
            foreach (DataRow row in dt.Rows)
            {
                orders.Add(new OrderBL
                {
                    order_id = Convert.ToInt32(row["order_id"]),
                    user_id = Convert.ToInt32(row["user_id"]),
                    restaurant_id = Convert.ToInt32(row["restaurant_id"]),
                    status = row["status"].ToString(),
                    delivery_address = row["delivery_address"].ToString(),
                    delivery_time = row["delivery_time"] == DBNull.Value ? null : (DateTime?)row["delivery_time"],
                    created_at = Convert.ToDateTime(row["created_at"])
                });
            }
            return orders;
        }

        public static void UpdateOrderStatus(int orderId, string status)
        {
            string query = @"UPDATE orders 
                     SET status = @Status 
                     WHERE order_id = @OrderId;";

            DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@Status", status),
                new MySqlParameter("@OrderId", orderId)
            );
        }

        public static int AddOrder(OrderBL order)
        {
        string query = @"INSERT INTO orders (user_id, restaurant_id, delivery_address, delivery_time, status) 
                    VALUES (@UserId, @RestaurantId, @Address, @DeliveryTime, @Status);
                    SELECT LAST_INSERT_ID();";

        DataTable dt = DatabaseHelper.ExecuteQuery(query,
            new MySqlParameter("@UserId", order.user_id),
            new MySqlParameter("@RestaurantId", order.restaurant_id),
            new MySqlParameter("@Address", order.delivery_address),
            new MySqlParameter("@DeliveryTime", order.delivery_time),
            new MySqlParameter("@Status", order.status));

        return Convert.ToInt32(dt.Rows[0][0]);
        }

        public static void AddOrderItems(int orderId, int cartId) 
        { 
            string query = @"INSERT INTO order_items (order_id, item_id, quantity, unit_price)
        SELECT @OrderId, ci.item_id, ci.quantity, mi.price FROM cart_items ci 
        INNER JOIN menu_items mi ON ci.item_id = mi.item_id WHERE ci.cart_id = @CartId"; 
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@OrderId", orderId),
                new MySqlParameter("@CartId", cartId));
        }
        public static decimal GetOrderTotal(int orderId)
        {
            string query = "SELECT SUM(quantity * unit_price) FROM order_items WHERE order_id=@OrderId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@OrderId", orderId));

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToDecimal(dt.Rows[0][0]); 
            }
            return 0;
        }
        public static List<OrderBL> GetNewOrders(int restaurantId)
        {
            string query = "SELECT * FROM orders WHERE restaurant_id=@RestaurantId AND restaurant_response='Pending'";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@RestaurantId", restaurantId));

            List<OrderBL> orders = new List<OrderBL>();
            foreach (DataRow row in dt.Rows)
            {
                orders.Add(new OrderBL
                {
                    order_id = Convert.ToInt32(row["order_id"]),
                    user_id = Convert.ToInt32(row["user_id"]),
                    restaurant_id = Convert.ToInt32(row["restaurant_id"]),
                    status = row["status"].ToString(),
                    delivery_address = row["delivery_address"].ToString(),
                    delivery_time = row["delivery_time"] == DBNull.Value ? null : (DateTime?)row["delivery_time"],
                    created_at = Convert.ToDateTime(row["created_at"])
                });
            }
            return orders;
        }
    }
}
