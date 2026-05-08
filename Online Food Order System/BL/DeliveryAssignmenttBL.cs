using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class DeliveryAssignmentBL
    {
        public int assignment_id { get; set; }
        public int order_id { get; set; }
        public int rider_id { get; set; }
        public string status { get; set; }
    }

}
