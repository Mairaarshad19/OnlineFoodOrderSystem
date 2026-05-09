using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class FeedbackBL
    {
        public int feedback_id { get; set; }
        public int user_id { get; set; }
        public int restaurant_id { get; set; }
        public int order_id { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
    }

}
