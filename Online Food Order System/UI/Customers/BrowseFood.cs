using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.DL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using Online_Food_Order_System.Helpers;

namespace Online_Food_Order_System.UI.Customers
{
    public partial class BrowseFood : Form
    {

        public BrowseFood()
        {
            InitializeComponent();
            LoadCuisineTypes();
            LoadCartGrid();
            

        }
        private List<dynamic> tempCart = new List<dynamic>();

        public List<CartItemBL> Items { get; set; } = new List<CartItemBL>();

        private void LoadCuisineTypes()
        {
            comboBoxCuisine.DataSource = CuisineDL.GetAllCuisines();
            comboBoxCuisine.DisplayMember = "name";
            comboBoxCuisine.ValueMember = "cuisine_id";
            comboBoxCuisine.SelectedIndex = -1;
        }


        private void BrowseFood_Load(object sender, EventArgs e)
        {
            List<RestaurantBL> restaurants = RestaurantDL.GetAllRestaurants();

            cmbRestaurant.DataSource = restaurants;
            cmbRestaurant.DisplayMember = "name";            
            cmbRestaurant.ValueMember = "restaurant_id";
            cmbRestaurant.SelectedIndex = -1;
            cmbFood.SelectedIndex = -1;
            updated_cart.Columns.Clear();
            updated_cart.Columns.Add("cart_item_id", "Cart Item ID"); 
            updated_cart.Columns["cart_item_id"].Visible = false; 
        }

        private void cmbRestaurant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRestaurant.SelectedItem != null)
            {
                RestaurantBL selectedRestaurant = (RestaurantBL)cmbRestaurant.SelectedItem;
                int restaurantId = selectedRestaurant.restaurant_id;
                List<MenuItemBL> menuItems = MenuItemDL.GetMenuItemsByRestaurant(restaurantId);
                dataGridViewFoodDetails.Rows.Clear(); 
                foreach (MenuItemBL item in menuItems) 
                { 
                    dataGridViewFoodDetails.Rows.Add( item.item_id, item.restaurant_id,
                        item.name, item.description, item.price, item.is_available ? "Available" : "Not Available" );
                }
                cmbFood.DataSource = menuItems; 
                cmbFood.DisplayMember = "name"; 
                cmbFood.ValueMember = "item_id"; 
            }
        }

        private void cmbFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFood.SelectedItem != null)
            {
                MenuItemBL selectedItem = (MenuItemBL)cmbFood.SelectedItem;

                dataGridViewFoodDetails.Rows.Clear();
                dataGridViewFoodDetails.Rows.Add(
                    selectedItem.name,
                    selectedItem.description,
                    selectedItem.price,
                    selectedItem.is_available ? "Available" : "Not Available"
                );
            }
        }

        private void buttonAddToCart_Click(object sender, EventArgs e)
        {
            
        }


        private void dataGridViewFoodDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Session.LoggedInUser == null)
            {
                MessageBox.Show("Please log in first.");
                return;
            }
            int userId = Session.LoggedInUser.user_id;

            if (cmbRestaurant.SelectedItem == null || cmbFood.SelectedItem == null)
            {
                MessageBox.Show("Please select a restaurant and a food item.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numericUpDownQuantity.Value <= 0)
            {
                MessageBox.Show("Please select a quantity greater than 0.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            MenuItemBL selectedItem = (MenuItemBL)cmbFood.SelectedItem;
            if (!selectedItem.is_available)
            {
                MessageBox.Show("This item is not available.",
                    "Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            CartBL userCart = CartDL.GetCartByUser(userId);
            if (userCart == null)
            {
                userCart = new CartBL { user_id = userId };
                CartDL.AddCart(userCart);
                userCart = CartDL.GetCartByUser(userId);
                MessageBox.Show($"Cart created with ID: {userCart.cart_id}");
            }

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = selectedItem.item_id,
                quantity = (int)numericUpDownQuantity.Value
            };

            try
            {
                CartItemDL.AddCartItem(cartItem);
                MessageBox.Show($"{selectedItem.name} (x{cartItem.quantity}) added to cart!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
                decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
                lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
                    LoadCartGrid();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        /*private void LoadCartGrid()
        {
            int userId = Session.LoggedInUser.user_id;
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            updated_cart.Rows.Clear();

            foreach (CartItemBL item in cartItems)
            {
                updated_cart.Rows.Add(
                    item.FoodName,
                    item.UnitPrice,
                    item.quantity,
                    item.TotalPrice
                );
            }
        }
        */


        private void LoadCartGrid()
        {
            int userId = Session.LoggedInUser.user_id;
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            updated_cart.DataSource = null;
            updated_cart.DataSource = cartItems;
        }


        private void updated_cart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Session.LoggedInUser == null)
            {
                MessageBox.Show("Please log in first.");
                return;
            }

            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            if (cartItems == null || cartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (cmbRestaurant.SelectedValue == null)
            {
                MessageBox.Show("Please select a restaurant.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int restaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);
            DateTime deliveryTime = DateTime.Now.AddMinutes(30);

            // Create order
            OrderBL newOrder = new OrderBL(userId, restaurantId, deliveryTime);
            int orderId = OrderDL.AddOrder(newOrder);

            // Add items from cart
            OrderDL.AddOrderItems(orderId, userCart.cart_id);

            // Open PaymentProcessing form with orderId + userCart
            this.Hide();
            PaymentProcessing paymentForm = new PaymentProcessing(orderId, userCart, cartItems);
            paymentForm.Show();
        }

        private void lblFinalTotal_Click(object sender, EventArgs e)
        {

        }
        // Update
        private void button11_Click(object sender, EventArgs e)
        {
            if (updated_cart.SelectedRows.Count > 0 && numericUpDownQuantity.Value != 0)
            { 
                // Get the bound object directly
                CartItemBL selectedItem = (CartItemBL)updated_cart.SelectedRows[0].DataBoundItem; 
                int newQuantity = (int)numericUpDownQuantity.Value; 
                CartItemDL.UpdateCartItemQuantity(selectedItem.cart_item_id, newQuantity);
                MessageBox.Show("Cart item updated successfully!", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                LoadCartGrid(); 
            } 
            else {
                MessageBox.Show("Please select an item to update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }
        }


        // Delete
        private void button12_Click(object sender, EventArgs e)
        {
            if (updated_cart.SelectedRows.Count > 0)
            {
                CartItemBL selectedItem = (CartItemBL)updated_cart.SelectedRows[0].DataBoundItem;
                CartItemDL.RemoveCartItem(selectedItem.cart_item_id);

                MessageBox.Show("Cart item deleted successfully!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCartGrid();
            }
            else
            {
                MessageBox.Show("Please select an item to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (Session.LoggedInUser == null)
            {
                MessageBox.Show("Please log in first.");
                return;
            }

            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);
            if (userCart != null)
            {
                CartDL.ClearCart(userCart.cart_id);
                MessageBox.Show("Cart cleared successfully!", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCartGrid();
            }
            else
            {
                MessageBox.Show("No cart found for this user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Login();
            form.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxCuisine_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void LoadCartGrid1()
        {
            updated_cart.DataSource = null;
            updated_cart.DataSource = tempCart;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tempCart.Add(new
            {
                FoodName = "Pizza",
                Quantity = 1,
                Price = 300m,
                Subtotal = 300m
            });

            LoadCartGrid();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tempCart.Add(new 
            { 
                FoodName = "Loaded Fries",
                Quantity = 1,
                Price = 250m,
                Subtotal = 250m });
            LoadCartGrid();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tempCart.Add(new { FoodName = "Fries", Quantity = 1, Price = 80m, Subtotal = 80m });
            LoadCartGrid();
        }

        private void button7_Click(object sender, EventArgs e)
        { 

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 4 || selectedRestaurantId != 4)
            {
                MessageBox.Show("Please select Fast Food cuisine and Usman’s Burger Hub to order Burger.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 4);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 41,
                FoodName = "Cheese Burger",
                UnitPrice = 450m,
                quantity = 1,
                TotalPrice = 450m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }


        private void button5_Click_1(object sender, EventArgs e)
        {

            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 9 || selectedRestaurantId != 9)
            {
                MessageBox.Show("Please select Pizza cuisine and Bilal’s Pizza Palace to order Pizza.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 9);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 91,
                FoodName = "Pepperoni Pizza",
                UnitPrice = 1200m,
                quantity = 1,
                TotalPrice = 1200m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }


        private void buttonCoffee_Click(object sender, EventArgs e)
        {

        }

// … and so on for Steak, Hot Wings, Biryani

        private void button9_Click_1(object sender, EventArgs e)
        {
            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 6 || selectedRestaurantId != 6)
            {
                MessageBox.Show("Please select Cafe cuisine and Usman’s Coffee Cafe to order Coffee.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 6);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 61,
                FoodName = "Cappuccino",
                UnitPrice = 250m,
                quantity = 1,
                TotalPrice = 250m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }



        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 1 || selectedRestaurantId != 1)
            {
                MessageBox.Show("Please select BBQ cuisine and Bilal’s BBQ Grill to order Steak.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 1);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 14,
                FoodName = "Mutton Chops",
                UnitPrice = 800m,
                quantity = 1,
                TotalPrice = 800m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }




        private void button7_Click_1(object sender, EventArgs e)
        {
            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 1 || selectedRestaurantId != 1)
            {
                MessageBox.Show("Please select BBQ cuisine and Bilal’s BBQ Grill to order Hot Wings.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 1);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 16,
                FoodName = "BBQ Wings",
                UnitPrice = 450m,
                quantity = 1,
                TotalPrice = 450m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            int userId = Session.LoggedInUser.user_id;
            CartBL userCart = CartDL.GetCartByUser(userId);

            int selectedCuisineId = Convert.ToInt32(comboBoxCuisine.SelectedValue);
            int selectedRestaurantId = Convert.ToInt32(cmbRestaurant.SelectedValue);

            if (selectedCuisineId != 10 || selectedRestaurantId != 10)
            {
                MessageBox.Show("Please select Pakistani cuisine and Usman’s Biryani House to order Biryani.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RestrictToOneRestaurant(userCart, 10);

            CartItemBL cartItem = new CartItemBL
            {
                cart_id = userCart.cart_id,
                item_id = 101,
                FoodName = "Chicken Biryani",
                UnitPrice = 350m,
                quantity = 1,
                TotalPrice = 350m
            };

            CartItemDL.AddCartItem(cartItem);
            List<CartItemBL> cartItems = CartItemDL.GetCartItemsByUser(userId);
            decimal finalTotal = cartItems.Sum(ci => ci.TotalPrice);
            lblFinalTotal.Text = $"Final Total: {finalTotal:C}";
            LoadCartGrid();
        }



        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxCuisine_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxCuisine.SelectedValue != null && comboBoxCuisine.SelectedValue is int)
            {
                int selectedCuisineId = (int)comboBoxCuisine.SelectedValue;

                // ✅ Load restaurants for this cuisine
                List<RestaurantBL> restaurants = RestaurantDL.GetRestaurantsByCuisine(selectedCuisineId);

                cmbRestaurant.DataSource = restaurants;
                cmbRestaurant.DisplayMember = "name";
                cmbRestaurant.ValueMember = "restaurant_id";

                // ✅ If at least one restaurant exists, load its food items
                if (restaurants.Count > 0)
                {
                    int firstRestaurantId = restaurants[0].restaurant_id;

                    List<MenuItemBL> menuItems = MenuItemDL.GetMenuItemsByRestaurant(firstRestaurantId);

                    cmbFood.DataSource = menuItems;
                    cmbFood.DisplayMember = "name";
                    cmbFood.ValueMember = "item_id";
                }
                else
                {
                    // Clear food combo if no restaurant found
                    cmbFood.DataSource = null;
                }
            }
        }
        private void RestrictToOneRestaurant(CartBL userCart, int newRestaurantId)
        {
            List<CartItemBL> existingItems = CartItemDL.GetCartItemsByCartId(userCart.cart_id);
            if (existingItems.Any())
            {
                int existingRestaurantId = MenuItemDL.GetRestaurantIdByItem(existingItems.First().item_id);
                if (existingRestaurantId != newRestaurantId)
                {
                    MessageBox.Show("You can only order from one restaurant at a time. Please checkout or clear your cart first.",
                        "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw new InvalidOperationException("Cart contains items from another restaurant.");
                }
            }
        }

    }
}



