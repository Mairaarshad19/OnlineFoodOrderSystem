using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;
using Online_Food_Order_System.UI.Customers;
using Online_Food_Order_System.UI.Restaurants;
using Online_Food_Order_System.UI.Admin;

namespace Online_Food_Order_System.UI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Main_Page();
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter email and password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxRole == null || comboBoxRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRole = comboBoxRole.SelectedItem.ToString();
            string hashedPassword = HashPassword(password);

            try
            {
                UserBL user = UserDL.AuthenticateUser(email, hashedPassword, selectedRole);

                if (user != null)
                {
                    Session.LoggedInUser = user;
                    MessageBox.Show($"{user.role} login successful!");

                    // Redirect based on role
                    if (user.role == "customer")
                    {
                        this.Hide();
                        new BrowseFood().Show();
                    }
                    else if (user.role == "restaurant")
                    {
                        RestaurantBL restaurant = RestaurantDL.GetRestaurantByOwner(user.user_id);

                        if (restaurant != null)
                        {
                            Session.LoggedInRestaurant = restaurant;
                            this.Hide();
                            new MenuManagement().Show();
                        }
                        else
                        {
                            MessageBox.Show("No restaurant found for this account.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (user.role == "admin")
                    {
                        // ✅ Set admin session
                        Session.LoggedInAdmin = new AdminBL
                        {
                            admin_id = user.user_id,
                            name = user.name,
                            email = user.email
                        };

                        this.Hide();
                        new Add_Restaurants().Show(); // replace Add_Restaurants with your main admin form
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email, password, or role selection.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
