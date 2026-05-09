using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.UI;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public static class OrderItemDL
    {
        public static List<OrderItemBL> GetOrderItems(int orderId)
        {
            string query = @"SELECT oi.order_item_id,
                                oi.order_id,
                                oi.item_id,
                                oi.quantity,
                                oi.unit_price,
                                (oi.quantity * oi.unit_price) AS total_price,
                                mi.name AS FoodName
                         FROM order_items oi
                         INNER JOIN menu_items mi ON oi.item_id = mi.item_id
                         WHERE oi.order_id = @OrderId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@OrderId", orderId));

            List<OrderItemBL> orderItems = new List<OrderItemBL>();
            foreach (DataRow row in dt.Rows)
            {
                orderItems.Add(new OrderItemBL
                {
                    order_item_id = Convert.ToInt32(row["order_item_id"]),
                    order_id = Convert.ToInt32(row["order_id"]),
                    item_id = Convert.ToInt32(row["item_id"]),
                    quantity = Convert.ToInt32(row["quantity"]),
                    unit_price = Convert.ToDecimal(row["unit_price"]),
                    total_price = Convert.ToDecimal(row["total_price"]),
                    FoodName = row["FoodName"].ToString()
                });
            }
            return orderItems;
        }
        public static void AddOrderItem(OrderItemBL item)
        {
            string query = "INSERT INTO order_items (order_id,item_id,quantity,unit_price) VALUES (@OrderId,@ItemId,@Quantity,@Price)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@OrderId", item.order_id),
                new MySqlParameter("@ItemId", item.item_id),
                new MySqlParameter("@Quantity", item.quantity),
                new MySqlParameter("@Price", item.unit_price));
        }

    }

}
