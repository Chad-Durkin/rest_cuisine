using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace RestaurantCuisine
{
    public class Cuisine
    {
        private int _id;
        private string _name;

        public Cuisine(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public override bool Equals(System.Object otherCuisine)
        {
            if(!(otherCuisine is Cuisine))
            {
                return false;
            }
            else
            {
                Cuisine newCuisine = (Cuisine) otherCuisine;
                bool idEquality = this.GetId() == newCuisine.GetId();
                bool nameEquality = this.GetName() == newCuisine.GetName();
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetName().GetHashCode();
        }

        public static List<Cuisine> GetAll()
        {
            List<Cuisine> allCuisine = new List<Cuisine>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int cuisineId = rdr.GetInt32(0);
                string cuisineName = rdr.GetString(1);
                Cuisine newCuisine = new Cuisine(cuisineName, cuisineId);
                allCuisine.Add(newCuisine);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allCuisine;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO cuisines (name) OUTPUT INSERTED.id VALUES (@CuisineName);", conn);

            cmd.Parameters.Add(new SqlParameter("@CuisineName", this.GetName()));
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            DB.CloseSqlConnection(rdr, conn);
        }

        public static Cuisine Find(int targetId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE id = @CuisineId;", conn);

            cmd.Parameters.Add(new SqlParameter("@CuisineId", targetId));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundName = null;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
            }
            Cuisine foundCuisine = new Cuisine(foundName, foundId);

            DB.CloseSqlConnection(rdr, conn);
            return foundCuisine;
        }

        public List<Restaurant> GetRestaurants()
        {
            List<Restaurant> cuisineRestaurants = new List<Restaurant>{};
            int cuisineId = this.GetId();

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

            cmd.Parameters.Add(new SqlParameter("@CuisineId", cuisineId));

            SqlDataReader rdr = cmd.ExecuteReader();


            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundName = rdr.GetString(1);
                string foundAddress = rdr.GetString(2);
                string foundPhoneNumber = rdr.GetString(3);
                cuisineRestaurants.Add(new Restaurant(foundName, foundAddress, foundPhoneNumber, cuisineId, foundId));
            }

            DB.CloseSqlConnection(rdr, conn);

            return cuisineRestaurants;
        }

        public int GetId()
        {
            return _id;
        }
        public void SetId(int newId)
        {
            _id = newId;
        }

        public string GetName()
        {
            return _name;
        }
        public void SetName(string newName)
        {
            _name = newName;
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("cuisines");
        }

    }
}
