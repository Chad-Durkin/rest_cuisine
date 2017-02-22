using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace RestaurantCuisine
{
    public class Restaurant
    {
        private int _id;
        private int _cuisineId;
        private string _name;
        private string _address;
        private string _phoneNumber;

        public Restaurant(string name, string address, string phoneNumber, int cuisineId, int id = 0)
        {
            _id = id;
            _name = name;
            _address = address;
            _phoneNumber = phoneNumber;
            _cuisineId = cuisineId;
        }

        //Override Equal and HashCode
        public override bool Equals(System.Object otherRestaurant)
        {
            if (!(otherRestaurant is Restaurant))
            {
                return false;
            }
            else
            {
                Restaurant newRestaurant = (Restaurant) otherRestaurant;
                bool idEquality = this.GetId() == newRestaurant.GetId();
                bool nameEquality = this.GetName() == newRestaurant.GetName();
                bool addressEquality = this.GetAddress() == newRestaurant.GetAddress();
                bool phoneNumberEquality = this.GetPhoneNumber() == newRestaurant.GetPhoneNumber();
                bool cuisineIdEquality = this.GetCuisineId() == newRestaurant.GetCuisineId();
                return(idEquality && nameEquality && addressEquality && phoneNumberEquality && cuisineIdEquality);
            }
        }
        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        //GetAll, Save, Find
        public static List<Restaurant> GetAll()
        {
            List<Restaurant> allRestaurants = new List<Restaurant>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int restaurantId = rdr.GetInt32(0);
                string restaurantName = rdr.GetString(1);
                string restaurantAddress = rdr.GetString(2);
                string restaurantPhoneNumber = rdr.GetString(3);
                int restaurantCuisineId = rdr.GetInt32(4);
                Restaurant newRestaurant = new Restaurant(restaurantName, restaurantAddress, restaurantPhoneNumber, restaurantCuisineId, restaurantId);
                allRestaurants.Add(newRestaurant);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allRestaurants;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, address, phone_number, cuisine_id) OUTPUT INSERTED.id VALUES (@Name, @Address, @PhoneNumber, @CuisineId);", conn);

            cmd.Parameters.Add(new SqlParameter("@Name", this.GetName()));
            cmd.Parameters.Add(new SqlParameter("@Address", this.GetAddress()));
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", this.GetPhoneNumber()));
            cmd.Parameters.Add(new SqlParameter("@CuisineId", this.GetCuisineId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static Restaurant Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @RestaurantId;", conn);

            cmd.Parameters.Add(new SqlParameter("@RestaurantId", id));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;
            string foundAddress = null;
            string foundPhoneNumber = null;
            int foundCuisineId = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
                foundAddress = rdr.GetString(2);
                foundPhoneNumber = rdr.GetString(3);
                foundCuisineId = rdr.GetInt32(4);
            }

            Restaurant foundRestaurant = new Restaurant(foundName, foundAddress, foundPhoneNumber, foundCuisineId, foundId);

            DB.CloseSqlConnection(rdr, conn);

            return foundRestaurant;
        }

        public int GetId()
        {
            return _id;
        }
        public string GetName()
        {
            return _name;
        }
        public string GetAddress()
        {
            return _address;
        }
        public string GetPhoneNumber()
        {
            return _phoneNumber;
        }
        public int GetCuisineId()
        {
            return _cuisineId;
        }
        public void SetId(int id)
        {
            _id = id;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public void SetAddress(string address)
        {
            _address = address;
        }
        public void SetPhoneNumber(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
        }
        public void SetCuisineId(int cuisineId)
        {
            _cuisineId = cuisineId;
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("restaurants");
        }
    }
}
