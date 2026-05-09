using MySql.Data.MySqlClient;
using Online_Food_Order_System.BL;
using Online_Food_Order_System.Online_Food_Order_System;
using System.Collections.Generic;
using System.Data;
using System;

public static class MenuItemDL
{
    // Create
    public static void AddMenuItem(MenuItemBL menuItem)
    {
        string query = @"INSERT INTO menu_items 
        (restaurant_id, name, description, price, is_available, estimated_delivery_time, cuisine_type) 
        VALUES (@RestaurantId, @Name, @Description, @Price, @IsAvailable, @DeliveryTime, @CuisineType)";

        DatabaseHelper.ExecuteNonQuery(query,
            new MySqlParameter("@RestaurantId", menuItem.restaurant_id),
            new MySqlParameter("@Name", menuItem.name),
            new MySqlParameter("@Description", menuItem.description ?? ""),
            new MySqlParameter("@Price", menuItem.price),
            new MySqlParameter("@IsAvailable", menuItem.is_available),
            new MySqlParameter("@DeliveryTime", menuItem.estimated_delivery_time),
            new MySqlParameter("@CuisineType", menuItem.cuisine_type ?? "General")
        );
    }


    public static int GetRestaurantIdByItem(int itemId) 
    { 
        string query = "SELECT restaurant_id FROM menu_items WHERE item_id=@ItemId"; 
        DataTable dt = DatabaseHelper.ExecuteQuery(query, 
            new MySqlParameter("@ItemId", itemId)); 
        if (dt.Rows.Count > 0) 
        { 
            return Convert.ToInt32(dt.Rows[0][0]);
        } 
        return 0;
    }

    // Read
    public static List<MenuItemBL> GetMenuItemsByRestaurant(int restaurantId)
    {
        string query = "SELECT * FROM menu_items WHERE restaurant_id=@RestaurantId AND is_available=TRUE";
        DataTable dt = DatabaseHelper.ExecuteQuery(query,
            new MySqlParameter("@RestaurantId", restaurantId));

        List<MenuItemBL> menuItems = new List<MenuItemBL>();
        foreach (DataRow row in dt.Rows)
        {
            menuItems.Add(new MenuItemBL
            {
                item_id = Convert.ToInt32(row["item_id"]),
                restaurant_id = Convert.ToInt32(row["restaurant_id"]),
                name = row["name"].ToString(),
                description = row["description"].ToString(),
                price = Convert.ToDecimal(row["price"]),
                is_available = Convert.ToBoolean(row["is_available"]),
                cuisine_type = row["cuisine_type"].ToString(),
                estimated_delivery_time = Convert.ToInt32(row["estimated_delivery_time"])
            });
        }
        return menuItems;
    }

    // Update
    public static void UpdateMenuItem(MenuItemBL menuItem)
    {
        string query = @"UPDATE menu_items 
        SET name=@Name, description=@Description, price=@Price, 
            is_available=@IsAvailable, estimated_delivery_time=@DeliveryTime, cuisine_type=@CuisineType
        WHERE item_id=@Id";

        DatabaseHelper.ExecuteNonQuery(query,
            new MySqlParameter("@Name", menuItem.name),
            new MySqlParameter("@Description", menuItem.description ?? ""),
            new MySqlParameter("@Price", menuItem.price),
            new MySqlParameter("@IsAvailable", menuItem.is_available),
            new MySqlParameter("@DeliveryTime", menuItem.estimated_delivery_time),
            new MySqlParameter("@CuisineType", menuItem.cuisine_type ?? "General"),
            new MySqlParameter("@Id", menuItem.item_id)
        );
    }
    public static List<string> GetAllCuisineTypes()
    {
        List<string> cuisines = new List<string>();
        string query = "SELECT DISTINCT cuisine_type FROM menu_items";

        DataTable dt = DatabaseHelper.ExecuteQuery(query);
        foreach (DataRow row in dt.Rows)
        {
            cuisines.Add(row["cuisine_type"].ToString());
        }
        return cuisines;
    }



    // Delete
    public static void DeleteMenuItem(int itemId)
    {
        string query = "DELETE FROM menu_items WHERE item_id=@ItemId";
        DatabaseHelper.ExecuteNonQuery(query,
            new MySqlParameter("@ItemId", itemId));
    }
    public static List<MenuItemBL> GetMenuItemsByRestaurantAndCuisine(int restaurantId, string cuisineType)
    {
        List<MenuItemBL> items = new List<MenuItemBL>();
        string query = @"
        SELECT item_id, restaurant_id, name, description, price, is_available, estimated_delivery_time, cuisine_type
        FROM menu_items
        WHERE restaurant_id = @RestaurantId AND cuisine_type = @CuisineType";

        DataTable dt = DatabaseHelper.ExecuteQuery(query,
            new MySqlParameter("@RestaurantId", restaurantId),
            new MySqlParameter("@CuisineType", cuisineType));

        foreach (DataRow row in dt.Rows)
        {
            items.Add(new MenuItemBL
            {
                item_id = Convert.ToInt32(row["item_id"]),
                restaurant_id = Convert.ToInt32(row["restaurant_id"]),
                name = row["name"].ToString(),
                description = row["description"].ToString(),
                price = Convert.ToDecimal(row["price"]),
                is_available = Convert.ToBoolean(row["is_available"]),
                estimated_delivery_time = Convert.ToInt32(row["estimated_delivery_time"]),
                cuisine_type = row["cuisine_type"].ToString()
            });
        }
        return items;
    }

}
