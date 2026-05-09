using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class MenuItemBL
    {
        public int item_id { get; set; }
        public int restaurant_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public bool is_available { get; set; }
        public int estimated_delivery_time { get; set; }
        public string cuisine_type { get; set; }   // ✅ string
    }

}
