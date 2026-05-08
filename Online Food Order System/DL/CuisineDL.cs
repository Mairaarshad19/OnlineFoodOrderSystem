using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;

namespace Online_Food_Order_System.DL
{
    public static class CuisineDL
    {
        public static void AddCuisine(CuisineBL cuisine)
        {
            string query = "INSERT INTO cuisines (name) VALUES (@Name)";

            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@Name", cuisine.name));
        }

        public static List<CuisineBL> GetAllCuisines()
        {
            string query = "SELECT * FROM cuisines";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            List<CuisineBL> cuisines = new List<CuisineBL>();
            foreach (DataRow row in dt.Rows)
            {
                cuisines.Add(new CuisineBL
                {
                    cuisine_id = Convert.ToInt32(row["cuisine_id"]),
                    name = row["name"].ToString()
                });
            }
            return cuisines;
        }
    }
}
