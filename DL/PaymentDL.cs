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
    public static class PaymentDL
    {
        public static void AddPayment(PaymentBL payment)
        {
            string query = "INSERT INTO payments (order_id, method, amount, status) " +
                            "VALUES (@OrderId, @Method, @Amount, @Status)";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@OrderId", payment.order_id),
                new MySqlParameter("@Method", payment.method),
                new MySqlParameter("@Amount", payment.amount),
                new MySqlParameter("@Status", payment.status));
        }


        public static List<PaymentBL> GetPaymentsByOrder(int orderId)
        {
            string query = "SELECT * FROM payments WHERE order_id=@OrderId";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@OrderId", orderId));

            List<PaymentBL> payments = new List<PaymentBL>();
            foreach (DataRow row in dt.Rows)
            {
                payments.Add(new PaymentBL
                {
                    payment_id = row["payment_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["payment_id"]),
                    order_id = Convert.ToInt32(row["order_id"]),
                    method = row["method"].ToString(),
                    amount = row["amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["amount"]),
                    status = row["status"].ToString()
                });
            }
            return payments;
        }
    }


}
