using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mysqlx.Crud;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;

namespace Online_Food_Order_System.UI.Customers
{
    public partial class PaymentProcessing : Form
    {
            public List<CartItemBL> Items { get; set; } = new List<CartItemBL>();
            private readonly int orderId;
            private CartBL userCart;

            // ✅ New constructor with cartItems
            public PaymentProcessing(int orderId, CartBL userCart, List<CartItemBL> cartItems)
            {
                InitializeComponent();

                this.orderId = orderId;
                this.userCart = userCart;
                this.Items = cartItems;

                // Load grid immediately
                LoadCartGrid();
            }

        private void LoadCartGrid()
        {
            dataGridViewOrderItems.Rows.Clear();

            foreach (CartItemBL item in Items)
            {
                dataGridViewOrderItems.Rows.Add(
                    item.FoodName,
                    item.UnitPrice,
                    item.quantity,
                    item.TotalPrice
                );
            }

            if (Items.Count > 0)
            {
                decimal finalTotal = Items.Sum(ci => ci.TotalPrice);
                lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            }
            else
            {
                lblFinalTotal.Text = "$0.00";
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBoxPayment.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Use the orderId passed into the constructor
            List<OrderItemBL> orderItems = OrderItemDL.GetOrderItems(orderId);

            if (orderItems == null || orderItems.Count == 0)
            {
                MessageBox.Show("No items found for this order.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string method = comboBoxPayment.SelectedItem.ToString();
            decimal finalTotal = orderItems.Sum(oi => oi.total_price);

            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";

            // ✅ Record payment for existing order
            PaymentBL payment = new PaymentBL(orderId, method, finalTotal);
            PaymentDL.AddPayment(payment);

            MessageBox.Show("Payment recorded successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            //orderId = Convert.ToInt32(dataGridViewOrderItems.SelectedRows[0].Cells["order_id"].Value);

            LoadPaymentGrid(orderId);
            // ✅ Clear the cart after payment
            CartDL.ClearCart(userCart.cart_id);
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PaymentProcessing_Load(object sender, EventArgs e)
        {
            // Optionally: preload payment methods
            comboBoxPayment.Items.Clear();
            comboBoxPayment.Items.Add("JazzCash");
            comboBoxPayment.Items.Add("Easypaisa");
            comboBoxPayment.Items.Add("HBLKonnect");
            comboBoxPayment.Items.Add("CreditCard");
            comboBoxPayment.Items.Add("DebitCard");
            comboBoxPayment.Items.Add("COD");
        }


        private void lblFinalTotal_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new BrowseFood();
            form.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadPaymentGrid(int orderId)
        {
            try
            {
                // ✅ Fetch payments for this order
                List<PaymentBL> payments = PaymentDL.GetPaymentsByOrder(orderId);

                // ✅ Bind to DataGridView
                dataGridViewPayments.DataSource = null; // clear old binding
                dataGridViewPayments.DataSource = payments;

                // ✅ Optional: set column headers for readability
                dataGridViewPayments.Columns["payment_id"].HeaderText = "Payment ID";
                dataGridViewPayments.Columns["order_id"].HeaderText = "Order ID";
                dataGridViewPayments.Columns["method"].HeaderText = "Payment Method";
                dataGridViewPayments.Columns["amount"].HeaderText = "Amount";
                dataGridViewPayments.Columns["status"].HeaderText = "Status";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
