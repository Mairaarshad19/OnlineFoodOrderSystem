using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
        public class AdminBL
        {
            public int admin_id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }

            public AdminBL() { }

            public AdminBL(int id, string name, string email, string phone)
            {
                this.admin_id = id;
                this.name = name;
                this.email = email;
                this.phone = phone;
            }
        }
}
