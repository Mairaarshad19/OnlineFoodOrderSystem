using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
        public class OrderBL
        {
            public int order_id { get; set; }
            public int user_id { get; set; }
            public int restaurant_id { get; set; }
            public string status { get; set; }              // Confirmed, Preparing, Out for Delivery, Delivered, Cancelled // Pending, Accepted, Rejected, Preparing
            public string delivery_address { get; set; }
            public DateTime? delivery_time { get; set; }
            public DateTime created_at { get; set; }
        public OrderBL()
        {

        }

        public OrderBL(int userId, int restaurantId, string address, DateTime deliveryTime)
        {
            user_id = userId;
            restaurant_id = restaurantId;
            status = "Confirmed";
            delivery_address = address;
            delivery_time = deliveryTime;
        }
        public OrderBL(int userId, int restaurantId, DateTime deliveryTime)
        {
            user_id = userId;
            restaurant_id = restaurantId;
            delivery_time = deliveryTime;
            status = "Confirmed";
        }
    }

}
