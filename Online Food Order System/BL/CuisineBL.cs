using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Online_Food_Order_System.BL
{
        public class CuisineBL
        {
            public int cuisine_id { get; set; }
            public string name { get; set; }

            public CuisineBL() { }

            public CuisineBL(int id, string name)
            {
                this.cuisine_id = id;
                this.name = name;
            }
        }
}
