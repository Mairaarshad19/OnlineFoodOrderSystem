using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class CartBL
    {
        public int cart_id { get; set; }
        public int user_id { get; set; }

        public CartBL() { }
        public CartBL(int cartId, int userId)
        {
            this.cart_id = cartId;
            this.user_id = userId;
        }
    public List<CartItemBL> Items { get; set; } = new List<CartItemBL>();

    }
}
