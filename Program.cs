using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Online_Food_Order_System.UI;
using Online_Food_Order_System.UI.Admin;
using Online_Food_Order_System.UI.Customers;
using Online_Food_Order_System.UI.Restaurants;

namespace Online_Food_Order_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
