using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Online_Food_Order_System.BL
{
    public class UserBL
    {
        public int user_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public string password_hash { get; set; }
        public string social_provider { get; set; } // Google, Facebook, or null
        public string role { get; set; } // customer, restaurant, rider, admin

        public UserBL() { }

        public UserBL(int userId)
        {
            this.user_id = userId;
        }
        public static UserBL LoggedInRestaurant { get; set; }

        public UserBL(string name, string email, string mobile, string passwordHash, string role, string socialProvider = null)
        {
            this.name = name;
            this.email = email;
            this.mobile_number = mobile;
            this.password_hash = passwordHash;
            this.role = role;
            this.social_provider = socialProvider;
        }

        public UserBL(int userId, string name, string email, string mobile, string passwordHash, string role, string socialProvider = null)
        {
            this.user_id = userId;
            this.name = name;
            this.email = email;
            this.mobile_number = mobile;
            this.password_hash = passwordHash;
            this.role = role;
            this.social_provider = socialProvider;
        }
    }

}
