using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class OrderItemBL
    {
        public int order_item_id { get; set; }
        public int order_id { get; set; }
        public int item_id { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_price { get; set; }
        public string FoodName { get; set; }   
    }

}
