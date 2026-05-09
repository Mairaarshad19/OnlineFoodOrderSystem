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
    public class CartDL
    {
        public static int GetCartRestaurantId(int cartId)
        {
            string query = @"SELECT DISTINCT mi.restaurant_id
                     FROM cart_items ci
                     INNER JOIN menu_items mi ON ci.item_id = mi.item_id
                     WHERE ci.cart_id = @CartId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@CartId", cartId));

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(dt.Rows[0][0]); 
            }
            return 0;
        }

        public static void AddCart(CartBL cart)
        {
            string query = "INSERT INTO carts (user_id) VALUES (@UserId)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@UserId", cart.user_id));
        }
        public static CartBL GetCartByUser(int userId)
        {
            CartBL cart = null;

            string query = "SELECT cart_id, user_id FROM carts WHERE user_id = @UserId LIMIT 1";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@UserId", userId));

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                cart = new CartBL
                {
                    cart_id = Convert.ToInt32(row["cart_id"]),
                    user_id = Convert.ToInt32(row["user_id"])
                };

                // ✅ Populate items for this cart
                cart.Items = CartItemDL.GetCartItemsByCartId(cart.cart_id);
            }

            return cart;
        }
        
        public static void ClearCart(int cartId)
        { 
                string query = "DELETE FROM cart_items WHERE cart_id=@CartId";
                DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@CartId", cartId)); 
        }
    }

}
