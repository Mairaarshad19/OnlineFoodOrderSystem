using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public class UserDL
    {

        public static void AddUser(UserBL user)
        {
            string query = "INSERT INTO users (name, email, mobile_number, password_hash, social_provider, role) " +
                           "VALUES (@Name, @Email, @Mobile, @Password, @SocialProvider, @Role)";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Name", user.name),
                new MySqlParameter("@Email", user.email),
                new MySqlParameter("@Mobile", user.mobile_number),
                new MySqlParameter("@Password", user.password_hash),
                new MySqlParameter("@SocialProvider", (object)user.social_provider ?? DBNull.Value),
                new MySqlParameter("@Role", user.role)
            );
        }

        // Update existing user
        public static void UpdateUser(UserBL user)
        {
            string query = "UPDATE users SET name=@Name, email=@Email, mobile_number=@Mobile, " +
                           "password_hash=@Password, role=@Role, social_provider=@SocialProvider " +
                           "WHERE user_id=@UserId";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Name", user.name),
                new MySqlParameter("@Email", user.email),
                new MySqlParameter("@Mobile", user.mobile_number),
                new MySqlParameter("@Password", user.password_hash),
                new MySqlParameter("@Role", user.role),
                new MySqlParameter("@SocialProvider", (object)user.social_provider ?? DBNull.Value),
                new MySqlParameter("@UserId", user.user_id)
            );
        }

        // Delete user by ID
        public static void DeleteUser(int userId)
        {
            string query = "DELETE FROM users WHERE user_id=@UserId";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@UserId", userId)
            );
        }

        // Get all users
        public static List<UserBL> GetAllUsers()
        {
            string query = "SELECT * FROM users";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            List<UserBL> userList = new List<UserBL>();

            foreach (DataRow row in dt.Rows)
            {
                userList.Add(new UserBL(
                    Convert.ToInt32(row["user_id"]),
                    row["name"].ToString(),
                    row["email"].ToString(),
                    row["mobile_number"].ToString(),
                    row["password_hash"].ToString(),
                    row["role"].ToString(),
                    row["social_provider"] != DBNull.Value ? row["social_provider"].ToString() : null
                ));
            }
            return userList;
        }

        // Get user by ID
        public static UserBL GetUserById(int userId)
        {
            string query = "SELECT * FROM users WHERE user_id=@UserId";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@UserId", userId)
            );

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new UserBL(
                    Convert.ToInt32(row["user_id"]),
                    row["name"].ToString(),
                    row["email"].ToString(),
                    row["mobile_number"].ToString(),
                    row["password_hash"].ToString(),
                    row["role"].ToString(),
                    row["social_provider"] != DBNull.Value ? row["social_provider"].ToString() : null
                );
            }
            return null;
        }

        // Authenticate user by email + password + role
        public static UserBL AuthenticateUser(string email, string passwordHash, string role)
        {
            string query = "SELECT * FROM users WHERE email=@Email AND password_hash=@PasswordHash AND role=@Role";


            DataTable dt = DatabaseHelper.ExecuteQuery(query,
               new MySqlParameter("@Email", email),
               new MySqlParameter("@PasswordHash", passwordHash),
               new MySqlParameter("@Role", role)
           );

            if (dt == null || dt.Rows.Count == 0)
                return null;


            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new UserBL(
                    Convert.ToInt32(row["user_id"]),
                    row["name"].ToString(),
                    row["email"].ToString(),
                    row["mobile_number"].ToString(),
                    row["password_hash"].ToString(),
                    row["role"].ToString(),
                    row["social_provider"] != DBNull.Value ? row["social_provider"].ToString() : null
                );
            }
            return null;
        }


        // Check if email already exists (to prevent duplicates)
        public static bool EmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM users WHERE email=@Email";
            DataTable dt = DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@Email", email)
            );

            return Convert.ToInt32(dt.Rows[0][0]) > 0;
        }
    }
}
