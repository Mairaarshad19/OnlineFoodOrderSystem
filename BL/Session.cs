using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
    // Program.cs or a static Session class
    public static class Session
    {
        public static UserBL LoggedInUser { get; set; }
        public static RestaurantBL LoggedInRestaurant { get; set; }
        public static AdminBL LoggedInAdmin { get; set; }

        //public static 
    }

}
