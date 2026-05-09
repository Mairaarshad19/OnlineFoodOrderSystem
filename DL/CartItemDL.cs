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
    public static class CartItemDL
    {

            public static List<dynamic> GetCartWithFoodDetails(int cartId)
            {
                string query = @"SELECT ci.cart_item_id, 
                                mi.name AS FoodName, 
                                ci.quantity, 
                                mi.price, 
                                (ci.quantity * mi.price) AS Subtotal
                         FROM cart_items ci
                         JOIN menu_items mi ON ci.item_id = mi.item_id
                         WHERE ci.cart_id = @CartId";

                DataTable dt = DatabaseHelper.ExecuteQuery(query,
                    new MySqlParameter("@CartId", cartId));

                var cartItems = new List<dynamic>();
                foreach (DataRow row in dt.Rows)
                {
                    cartItems.Add(new
                    {
                        cart_item_id = Convert.ToInt32(row["cart_item_id"]),
                        FoodName = row["FoodName"].ToString(),
                        Quantity = Convert.ToInt32(row["quantity"]),
                        Price = Convert.ToDecimal(row["price"]),
                        Subtotal = Convert.ToDecimal(row["Subtotal"])
                    });
                }
                return cartItems;
            }

        public static void AddCartItem(CartItemBL cartItem)
         {
             int existingRestaurantId = CartDL.GetCartRestaurantId(cartItem.cart_id);
             int newItemRestaurantId = MenuItemDL.GetRestaurantIdByItem(cartItem.item_id);
             if (existingRestaurantId != 0 && existingRestaurantId != newItemRestaurantId)
             {
                 throw new InvalidOperationException("You can only order from one restaurant at a time. Please checkout or clear your cart first.");
             }

             string query = @"INSERT INTO cart_items (cart_id, item_id, quantity)
                          VALUES (@CartId, @ItemId, @Quantity)";

             DatabaseHelper.ExecuteNonQuery(query,
                 new MySqlParameter("@CartId", cartItem.cart_id),
                 new MySqlParameter("@ItemId", cartItem.item_id),
                 new MySqlParameter("@Quantity", cartItem.quantity));
         }
        
        // Remove item from cart
        public static void RemoveCartItem(int cartItemId)
        {
            string query = "DELETE FROM cart_items WHERE cart_item_id = @CartItemId";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@CartItemId", cartItemId));
        }
        // Update quantity
        public static void UpdateCartItemQuantity(int cartItemId, int newQuantity)
        {
            string query = "UPDATE cart_items SET quantity = @Quantity WHERE cart_item_id = @CartItemId";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Quantity", newQuantity),
                new MySqlParameter("@CartItemId", cartItemId));
        }

        public static DataTable GetCartDetails(int userId)
        {
            string query = @" SELECT mi.name AS FoodName, mi.price AS UnitPrice,
            ci.quantity AS Quantity, (mi.price * ci.quantity) 
            AS TotalPrice FROM carts c INNER JOIN cart_items ci ON
            c.cart_id = ci.cart_id INNER JOIN menu_items mi ON ci.item_id = mi.item_id 
            WHERE c.user_id = @UserId";
            return DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@UserId", userId));
        }

        public static List<CartItemBL> GetCartItems(int cartId)
        {
            string query = @"SELECT ci.cart_item_id,
               ci.cart_id,
               ci.item_id,
               ci.quantity,
               mi.name AS FoodName,
               mi.price AS UnitPrice,
               (ci.quantity * mi.price) AS TotalPrice
        FROM cart_items ci
        INNER JOIN menu_items mi ON ci.item_id = mi.item_id
        WHERE ci.cart_id = @CartId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@CartId", cartId));

            List<CartItemBL> cartItems = new List<CartItemBL>();
            foreach (DataRow row in dt.Rows)
            {
                cartItems.Add(new CartItemBL
                {
                    cart_item_id = Convert.ToInt32(row["cart_item_id"]),
                    cart_id = Convert.ToInt32(row["cart_id"]),
                    item_id = Convert.ToInt32(row["item_id"]),
                    quantity = Convert.ToInt32(row["quantity"]),
                    FoodName = row["FoodName"].ToString(),
                    UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                    TotalPrice = Convert.ToDecimal(row["TotalPrice"])
                });
            }
            return cartItems;
        }
      
        public static List<CartItemBL> GetCartItemsByUser(int userId)
        {
            string query = @"SELECT ci.cart_item_id, ci.cart_id, ci.item_id, ci.quantity,
               mi.name AS FoodName, mi.price AS UnitPrice,
               (mi.price * ci.quantity) AS TotalPrice
                FROM carts c
                INNER JOIN cart_items ci ON c.cart_id = ci.cart_id
                INNER JOIN menu_items mi ON ci.item_id = mi.item_id
                WHERE c.user_id = @UserId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@UserId", userId));

            List<CartItemBL> cartItems = new List<CartItemBL>();
            foreach (DataRow row in dt.Rows)
            {
                cartItems.Add(new CartItemBL
                {
                    cart_item_id = Convert.ToInt32(row["cart_item_id"]),
                    cart_id = Convert.ToInt32(row["cart_id"]),
                    item_id = Convert.ToInt32(row["item_id"]),
                    quantity = Convert.ToInt32(row["quantity"]),
                    FoodName = row["FoodName"].ToString(),
                    UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                    TotalPrice = Convert.ToDecimal(row["TotalPrice"])
                });
            }
            return cartItems;
        }
            public static List<CartItemBL> GetCartItemsByCartId(int cartId)
            {
                List<CartItemBL> cartItems = new List<CartItemBL>();

                string query = @"
                SELECT ci.cart_item_id, ci.cart_id, ci.item_id, ci.quantity,
                       m.name AS FoodName, m.price AS UnitPrice,
                       (ci.quantity * m.price) AS TotalPrice
                FROM cart_items ci
                INNER JOIN menu_items m ON ci.item_id = m.item_id
                WHERE ci.cart_id = @CartId";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@CartId", cartId));

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        CartItemBL item = new CartItemBL
                        {
                            cart_item_id = Convert.ToInt32(row["cart_item_id"]),
                            cart_id = Convert.ToInt32(row["cart_id"]),
                            item_id = Convert.ToInt32(row["item_id"]),
                            quantity = Convert.ToInt32(row["quantity"]),
                            FoodName = row["FoodName"].ToString(),
                            UnitPrice = Convert.ToDecimal(row["UnitPrice"]),
                            TotalPrice = Convert.ToDecimal(row["TotalPrice"])
                        };

                        cartItems.Add(item);
                    }
                }

                return cartItems;
            }
    }
}

