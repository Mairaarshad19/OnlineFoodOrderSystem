using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;
using Org.BouncyCastle.Asn1.Cmp;

namespace Online_Food_Order_System.UI.Restaurants
{
    public partial class OrderManagement : Form
    {
        public OrderManagement()
        {
            InitializeComponent();
        }

        private void OrderManagement_Load(object sender, EventArgs e)
        { 
            List<RestaurantBL> restaurants = RestaurantDL.GetAllRestaurants();
            cmbRestaurant.DataSource = restaurants; 
            cmbRestaurant.DisplayMember = "name";
            cmbRestaurant.ValueMember = "restaurant_id";
        }

        private void LoadOrdersByRestaurant(int restaurantId)
        {
            List<OrderBL> orders = OrderDL.GetOrdersByRestaurant(restaurantId);

            dataGridViewOrders.DataSource = null;
            dataGridViewOrders.DataSource = orders;

            // Hide technical columns
            if (dataGridViewOrders.Columns["order_id"] != null)
                dataGridViewOrders.Columns["order_id"].Visible = false;
            if (dataGridViewOrders.Columns["user_id"] != null)
                dataGridViewOrders.Columns["user_id"].Visible = false;
            if (dataGridViewOrders.Columns["restaurant_id"] != null)
                dataGridViewOrders.Columns["restaurant_id"].Visible = false;
            
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {

        }

        private void buttonReject_Click(object sender, EventArgs e)
        {

        }

        private void buttonPreparing_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0 && comboboxStatus.SelectedItem != null)
            {
                int orderId = Convert.ToInt32(dataGridViewOrders.SelectedRows[0].Cells["order_id"].Value);
                string newStatus = comboboxStatus.SelectedItem.ToString();
                OrderDL.UpdateOrderStatus(orderId, newStatus);
                MessageBox.Show($"Order status updated to {newStatus}!");
                int restaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);
                LoadOrdersByRestaurant(restaurantId);
            } 
        }

        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void cmbRestaurant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRestaurant.SelectedItem != null)
            {
                RestaurantBL selectedRestaurant = (RestaurantBL)cmbRestaurant.SelectedItem;
                int restaurantId = selectedRestaurant.restaurant_id;
                LoadOrdersByRestaurant(restaurantId);
            }

        }

        private void cmbstatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            var form = new MenuManagement();
            form.ShowDialog();
        }
    }
}
