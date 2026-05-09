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
using Online_Food_Order_System.Online_Food_Order_System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Online_Food_Order_System.UI.Restaurants
{
    public partial class MenuManagement : Form
    {
        public MenuManagement()
        {
            InitializeComponent();
            LoadMenuGrid();
            LoadRestaurantsComboBox();
            LoadCuisineTypesComboBox();
        }

        private void dataGridViewMenu_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewMenu.SelectedRows.Count > 0)
            {
                MenuItemBL selectedItem = (MenuItemBL)dataGridViewMenu.SelectedRows[0].DataBoundItem;

                txtName.Text = selectedItem.name;
                txtDescription.Text = selectedItem.description;
                txtPrice.Text = selectedItem.price.ToString("F2");


                // ✅ Use SelectedValue for cuisine binding
                if (comboBoxCuisine.DataSource != null)
                    comboBoxCuisine.SelectedValue = selectedItem.cuisine_type;

                num_time.Value = selectedItem.estimated_delivery_time;
                checkBox1.Checked = selectedItem.is_available;
            }
        }
        private void LoadCuisineTypesComboBox()
        {
            // Fetch cuisines from DB (returns List<CuisineBL>)
            List<CuisineBL> cuisines = CuisineDL.GetAllCuisines();

            // Bind to ComboBox
            comboBoxCuisine.DataSource = cuisines;
            comboBoxCuisine.DisplayMember = "name";       // what user sees
            comboBoxCuisine.ValueMember = "cuisine_id";   // actual FK value
            comboBoxCuisine.SelectedIndex = -1;           // no selection initially
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    MessageBox.Show("Please enter a valid name and price.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MenuItemBL newItem = new MenuItemBL
                {
                    restaurant_id = Session.LoggedInRestaurant.restaurant_id,
                    name = txtName.Text.Trim(),
                    description = txtDescription.Text.Trim(),
                    price = price,   
                    is_available = checkBox1.Checked,
                    cuisine_type = comboBoxCuisine.SelectedItem?.ToString() ?? "General",
                    estimated_delivery_time = (int)num_time.Value
                };



                MenuItemDL.AddMenuItem(newItem);

                MessageBox.Show("Menu item added successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadMenuGrid();
                ClearFormFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding menu item: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            txtName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            comboBoxCuisine.SelectedIndex = -1;
            num_time.Value = 30; // ✅ reset to default instead of 0
            checkBox1.Checked = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewMenu.SelectedRows.Count == 0) return;

            MenuItemBL selectedItem = (MenuItemBL)dataGridViewMenu.SelectedRows[0].DataBoundItem;

            selectedItem.name = txtName.Text.Trim();
            selectedItem.description = txtDescription.Text.Trim();
            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            selectedItem.price = price;

            selectedItem.cuisine_type = comboBoxCuisine.SelectedItem?.ToString();
            // ✅ correct binding
            selectedItem.estimated_delivery_time = (int)num_time.Value;
            selectedItem.is_available = checkBox1.Checked;

            MenuItemDL.UpdateMenuItem(selectedItem);
            LoadMenuGrid();
        }



        private void LoadMenuGrid()
        {
            // Get menu items for the logged-in restaurant
            int restaurantId = Session.LoggedInRestaurant.restaurant_id;
            List<MenuItemBL> menuItems = MenuItemDL.GetMenuItemsByRestaurant(restaurantId);

            dataGridViewMenu.DataSource = null;
            dataGridViewMenu.AutoGenerateColumns = true;
            dataGridViewMenu.DataSource = menuItems;
        }

       


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewMenu.SelectedRows.Count == 0) return;

            MenuItemBL selectedItem = (MenuItemBL)dataGridViewMenu.SelectedRows[0].DataBoundItem;

            MenuItemDL.DeleteMenuItem(selectedItem.item_id);
            LoadMenuGrid();
        }
        private void LoadRestaurantsComboBox()
        {
            // Get all restaurants from DL
            List<RestaurantBL> restaurants = RestaurantDL.GetAllRestaurants();

            if (restaurants == null || restaurants.Count == 0)
            {
                MessageBox.Show("No restaurants found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Bind to ComboBox
            cmbRestaurant.DataSource = restaurants;
            cmbRestaurant.DisplayMember = "name";          // what user sees
            cmbRestaurant.ValueMember = "restaurant_id";   // underlying ID
            cmbRestaurant.SelectedIndex = -1;              // no selection initially
        }

        private void MenuManagement_Load(object sender, EventArgs e)
        {
            num_time.Minimum = 10;   // minimum 10 minutes
            num_time.Maximum = 120;  // maximum 120 minutes
            num_time.Increment = 5;  // step size (5 minutes)
            num_time.Value = 30;     // default value
        }

        private void comboBoxCuisine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewMenu_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbRestaurant_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Login();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            var form = new OrderManagement();
            form.ShowDialog();
        }
    }
}
