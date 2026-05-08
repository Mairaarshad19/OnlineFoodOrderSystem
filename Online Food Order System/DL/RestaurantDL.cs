using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public class RestaurantDL
    {
        // ✅ Get Restaurant by Owner
        public static RestaurantBL GetRestaurantByOwner(int ownerId)
        {
            string query = "SELECT * FROM restaurants WHERE owner_id=@OwnerId";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@OwnerId", ownerId));

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new RestaurantBL(
                    Convert.ToInt32(row["restaurant_id"]),
                    Convert.ToInt32(row["owner_id"]),
                    row["name"].ToString(),
                    row["address"].ToString(),
                    row["email"].ToString(),
                    row["phone"].ToString(),
                    row["cuisine_id"] != DBNull.Value ? Convert.ToInt32(row["cuisine_id"]) : 0
                );
            }
            return null;
        }

        // ✅ Add Restaurant
        public static void AddRestaurant(RestaurantBL restaurant)
        {
            string query = "INSERT INTO restaurants (owner_id, name, address, email, phone, cuisine_id) " +
                           "VALUES (@OwnerId, @Name, @Address, @Email, @Phone, @CuisineId)";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@OwnerId", restaurant.owner_id),
                new MySqlParameter("@Name", restaurant.name),
                new MySqlParameter("@Address", restaurant.address),
                new MySqlParameter("@Email", restaurant.email),
                new MySqlParameter("@Phone", restaurant.phone),
                new MySqlParameter("@CuisineId", restaurant.cuisine_id)
            );
        }

        // ✅ Update Restaurant
        public static void UpdateRestaurant(RestaurantBL restaurant)
        {
            string query = "UPDATE restaurants SET name=@Name, address=@Address, email=@Email, phone=@Phone, cuisine_id=@CuisineId " +
                           "WHERE restaurant_id=@Id";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Name", restaurant.name),
                new MySqlParameter("@Address", restaurant.address),
                new MySqlParameter("@Email", restaurant.email),
                new MySqlParameter("@Phone", restaurant.phone),
                new MySqlParameter("@CuisineId", restaurant.cuisine_id),
                new MySqlParameter("@Id", restaurant.restaurant_id)
            );
        }

        // ✅ Delete Restaurant
        public static void DeleteRestaurant(int restaurantId)
        {
            string query = "DELETE FROM restaurants WHERE restaurant_id=@Id";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Id", restaurantId)
            );
        }

        // ✅ Get All Restaurants
        public static List<RestaurantBL> GetAllRestaurants()
        {
            string query = "SELECT * FROM restaurants";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            List<RestaurantBL> restaurants = new List<RestaurantBL>();
            foreach (DataRow row in dt.Rows)
            {
                int restaurantId = row["restaurant_id"] != DBNull.Value ? Convert.ToInt32(row["restaurant_id"]) : 0;
                int ownerId = row["owner_id"] != DBNull.Value ? Convert.ToInt32(row["owner_id"]) : 0;
                int cuisineId = row["cuisine_id"] != DBNull.Value ? Convert.ToInt32(row["cuisine_id"]) : 0;

                string name = row["name"] != DBNull.Value ? row["name"].ToString() : string.Empty;
                string address = row["address"] != DBNull.Value ? row["address"].ToString() : string.Empty;
                string email = row["email"] != DBNull.Value ? row["email"].ToString() : string.Empty;
                string phone = row["phone"] != DBNull.Value ? row["phone"].ToString() : string.Empty;

                restaurants.Add(new RestaurantBL(
                    restaurantId,
                    ownerId,
                    name,
                    address,
                    email,
                    phone,
                    cuisineId
                ));
            }
            return restaurants;
        }

        // ✅ Get Restaurant By ID
        public static RestaurantBL GetRestaurantById(int restaurantId)
        {
            string query = "SELECT * FROM restaurants WHERE restaurant_id=@Id";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@Id", restaurantId)
            );

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new RestaurantBL(
                    Convert.ToInt32(row["restaurant_id"]),
                    Convert.ToInt32(row["owner_id"]),
                    row["name"].ToString(),
                    row["address"].ToString(),
                    row["email"].ToString(),
                    row["phone"].ToString(),
                    row["cuisine_id"] != DBNull.Value ? Convert.ToInt32(row["cuisine_id"]) : 0
                );
            }
            return null;
        }

        // ✅ Get Restaurants By CuisineId
        public static List<RestaurantBL> GetRestaurantsByCuisine(int cuisineId)
        {
            List<RestaurantBL> restaurants = new List<RestaurantBL>();
            string query = "SELECT * FROM restaurants WHERE cuisine_id=@CuisineId";

            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@CuisineId", cuisineId));
            foreach (DataRow row in dt.Rows)
            {
                restaurants.Add(new RestaurantBL(
                    Convert.ToInt32(row["restaurant_id"]),
                    row["owner_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["owner_id"]),
                    row["name"] == DBNull.Value ? string.Empty : row["name"].ToString(),
                    row["address"] == DBNull.Value ? string.Empty : row["address"].ToString(),
                    row["email"] == DBNull.Value ? string.Empty : row["email"].ToString(),
                    row["phone"] == DBNull.Value ? string.Empty : row["phone"].ToString(),
                    row["cuisine_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["cuisine_id"])
                ));

            }
            return restaurants;
        }
    }
}
