using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{

    public class CartItemBL
    {
        public int cart_item_id { get; set; }
        public int cart_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; } 
        public string FoodName { get; set; } 
        public decimal UnitPrice { get; set; } 
        public decimal TotalPrice { get; set; } 

    }
}
