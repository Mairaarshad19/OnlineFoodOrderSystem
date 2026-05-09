using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    public class PaymentBL
    {
        public int payment_id { get; set; }
        public int order_id { get; set; }
        public string method { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; }
        public PaymentBL() { }
        public PaymentBL(int orderId, string method, decimal amount, string status = "Pending")
        {
            this.order_id = orderId;
            this.method = method;
            this.amount = amount;
            this.status = status;
        }
            public PaymentBL(int orderId, string method, decimal amount)
            {
                order_id = orderId;
                this.method = method;
                this.amount = amount;
                status = "Completed";
            }
        }
}
