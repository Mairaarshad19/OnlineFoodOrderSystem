using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Online_Food_Order_System.BL;

namespace Online_Food_Order_System.Helpers
{
    public static class SessionHelper
    {
        public static int GetLoggedInUserId()
        {
            if (Session.LoggedInUser == null)
                throw new InvalidOperationException("No user is logged in.");
            return Session.LoggedInUser.user_id;
        }

        public static int GetLoggedInRestaurantId()
        {
            if (Session.LoggedInRestaurant == null)
                throw new InvalidOperationException("No restaurant is logged in.");
            return Session.LoggedInRestaurant.restaurant_id;
        }

        public static int GetLoggedInAdminId()
        {
            if (Session.LoggedInAdmin == null)
                throw new InvalidOperationException("No admin is logged in.");
            return Session.LoggedInAdmin.admin_id;
        }
    }

}
