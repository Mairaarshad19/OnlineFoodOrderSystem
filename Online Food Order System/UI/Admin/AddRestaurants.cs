using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;

namespace Online_Food_Order_System.UI.Admin
{
    public partial class Add_Restaurants : Form
    {
        public Add_Restaurants()
        {
            InitializeComponent();
            LoadCuisineTypesComboBox();
        }
        private int hiddenRestaurantId = 0;

        private void LoadRestaurants()
        {
            // Fetch all restaurants from DL
            List<RestaurantBL> restaurants = RestaurantDL.GetAllRestaurants();

            // Bind to DataGridView
            dataGridViewRestaurants.DataSource = null;
            dataGridViewRestaurants.AutoGenerateColumns = true;
            dataGridViewRestaurants.DataSource = restaurants;

            // ✅ Hide the ID column
            if (dataGridViewRestaurants.Columns["restaurant_id"] != null)
            {
                dataGridViewRestaurants.Columns["restaurant_id"].Visible = false;
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

        private void button10_Click(object sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Restaurant name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int selectedCuisineId = (int)comboBoxCuisine.SelectedValue;

            // Build Restaurant object
            RestaurantBL newRestaurant = new RestaurantBL
            {
                name = txtName.Text.Trim(),
                address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim(),
                email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                cuisine_id = selectedCuisineId,
                owner_id = Session.LoggedInUser.user_id  // link to current logged-in user
            };

            try
            {
                // Insert into DB
                RestaurantDL.AddRestaurant(newRestaurant);

                MessageBox.Show("Restaurant added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh grid or combo box
                LoadRestaurants();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding restaurant: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewRestaurants_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { 
            if (e.RowIndex >= 0) // ensure not header row
            {
                DataGridViewRow row = dataGridViewRestaurants.Rows[e.RowIndex];

                txtName.Text = row.Cells["name"].Value?.ToString();
                txtAddress.Text = row.Cells["address"].Value?.ToString();
                txtEmail.Text = row.Cells["email"].Value?.ToString();
                txtPhone.Text = row.Cells["phone"].Value?.ToString();

                // Cuisine binding (if comboBoxCuisine is bound to cuisines list)
                if (row.Cells["cuisine_id"].Value != null)
                {
                    comboBoxCuisine.SelectedValue = Convert.ToInt32(row.Cells["cuisine_id"].Value);
                }

                // Store selected restaurant_id for update/delete
                hiddenRestaurantId = Convert.ToInt32(row.Cells["restaurant_id"].Value);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Login();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (hiddenRestaurantId == 0)
            {
                MessageBox.Show("Please select a restaurant first.");
                return;
            }

            RestaurantBL restaurant = new RestaurantBL
            {
                restaurant_id = hiddenRestaurantId,
                owner_id = Session.LoggedInUser.user_id, // or keep existing owner
                name = txtName.Text.Trim(),
                address = txtAddress.Text.Trim(),
                email = txtEmail.Text.Trim(),
                phone = txtPhone.Text.Trim(),
                cuisine_id = (int)comboBoxCuisine.SelectedValue
            };

            try
            {
                RestaurantDL.UpdateRestaurant(restaurant);
                MessageBox.Show("Restaurant updated successfully!");
                LoadRestaurants(); // refresh grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating restaurant: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (hiddenRestaurantId == 0)
            {
                MessageBox.Show("Please select a restaurant first.");
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this restaurant?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    RestaurantDL.DeleteRestaurant(hiddenRestaurantId);
                    MessageBox.Show("Restaurant deleted successfully!");
                    LoadRestaurants(); // refresh grid
                    ClearFields();     // optional: clear textboxes
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting restaurant: " + ex.Message);
                }
            }
        }
        private void ClearFields()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            comboBoxCuisine.SelectedIndex = -1;
            hiddenRestaurantId = 0;
        }

        private void comboBoxCuisine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
