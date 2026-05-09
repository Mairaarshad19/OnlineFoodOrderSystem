namespace Online_Food_Order_System.BL
{
    public class RestaurantBL
    {
        public int restaurant_id { get; set; }
        public int owner_id { get; set; }   // FK to users.user_id
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int cuisine_id { get; set; }   // ✅ NEW: FK to cuisines.cuisine_id

        public RestaurantBL() { }

        // Constructor with only restaurant_id
        public RestaurantBL(int restaurantId)
        {
            this.restaurant_id = restaurantId;
        }

        // Constructor with all fields (useful when fetching from DB)
        public RestaurantBL(int restaurantId, int ownerId, string name, string address, string email, string phone, int cuisineId)
        {
            this.restaurant_id = restaurantId;
            this.owner_id = ownerId;
            this.name = name;
            this.address = address;
            this.email = email;
            this.phone = phone;
            this.cuisine_id = cuisineId;
        }

        // Constructor without ID (useful when adding new restaurant before DB assigns ID)
        public RestaurantBL(int ownerId, string name, string address, string email, string phone, int cuisineId)
        {
            this.owner_id = ownerId;
            this.name = name;
            this.address = address;
            this.email = email;
            this.phone = phone;
            this.cuisine_id = cuisineId;
        }
    }
}
