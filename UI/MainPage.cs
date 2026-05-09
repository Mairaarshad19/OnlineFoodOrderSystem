using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;

namespace Online_Food_Order_System.UI
{
    public partial class Main_Page : Form
    {
        public Main_Page()
        {
            InitializeComponent();
            comboBoxSocial.Items.Clear();
            comboBoxSocial.Items.Add("Google");
            comboBoxSocial.Items.Add("Facebook");
            comboBoxSocial.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxRole.SelectedIndex = -1;
            comboBoxSocial.SelectedIndex = 0; // default to blank

        }

        private void Main_Page_Load(object sender, EventArgs e)
        {

        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2")); // convert to hex
                }
                return builder.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;                 
            string email = txtEmail.Text;                
            string mobile = txtMobile.Text;
            string Password = txtPassword.Text;
            string socialProvider = comboBoxSocial.SelectedItem?.ToString();
            string role = comboBoxRole.SelectedItem?.ToString();


            // Basic validation
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(mobile) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(socialProvider))
            {
                socialProvider = null; 
            }

            string hashedPassword = HashPassword(Password);

            try
            {
                //Create UserBL object
                UserBL newUser = new UserBL(name, email, mobile, hashedPassword, role, socialProvider);
                UserDL.AddUser(newUser);

                MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtName.Clear();
                txtEmail.Clear();
                txtMobile.Clear();
                txtPassword.Clear();
                comboBoxSocial.SelectedIndex = -1;
                comboBoxRole.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating account: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void Main_Page_Load_1(object sender, EventArgs e)
        {

        }

        private void comboBoxSocial_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Login();
            form.ShowDialog();
        }
    }
}
