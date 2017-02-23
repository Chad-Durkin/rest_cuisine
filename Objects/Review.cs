using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace RestaurantCuisine
{
    public class Review
    {
        private int _id;
        private string _review;
        private int _stars;
        private int _restaurantId;

        public Review(string review, int stars, int restaurantId, int id = 0)
        {
            _review = review;
            _stars = stars;
            _restaurantId = restaurantId;
            _id = id;
        }

        public override bool Equals(System.Object otherReview)
        {
            if(!(otherReview is Review))
            {
                return false;
            }
            else{
                Review newReview = (Review) otherReview;;
                bool idEquality = this.GetId() == newReview.GetId();
                bool reviewEquality = this.GetReview() == newReview.GetReview();
                bool starsEquality = this.GetStars() == newReview.GetStars();
                bool restaurantIdEquality = this.GetRestaurantId() == newReview.GetRestaurantId();
                return(idEquality && reviewEquality && starsEquality && restaurantIdEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetReview().GetHashCode();
        }

        public static List<Review> GetAll()
        {
            List<Review> allReviews = new List<Review>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int foundId = rdr.GetInt32(0);
                string foundReview = rdr.GetString(1);
                int foundStars = rdr.GetInt32(2);
                int foundRestaurantId = rdr.GetInt32(3);
                Review newReview = new Review(foundReview, foundStars, foundRestaurantId, foundId);
                allReviews.Add(newReview);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allReviews;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO reviews (review, stars, restaurant_id) OUTPUT INSERTED.id VALUES (@Review, @Stars, @RestaurantId);", conn);

            cmd.Parameters.Add(new SqlParameter("@Review", this.GetReview()));
            cmd.Parameters.Add(new SqlParameter("@Stars", this.GetStars()));
            cmd.Parameters.Add(new SqlParameter("@RestaurantId", this.GetRestaurantId()));

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }

            DB.CloseSqlConnection(rdr, conn);
        }

        public static List<Review> FindAll(int restaurantId)
        {
            List<Review> allReviews = new List<Review>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE restaurant_id = @RestaurantId;", conn);

            cmd.Parameters.Add(new SqlParameter("@RestaurantId", restaurantId));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundReview = null;
            int foundStars = 0;
            int foundRestaurantId = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundReview = rdr.GetString(1);
                foundStars = rdr.GetInt32(2);
                foundRestaurantId = rdr.GetInt32(3);
                Review newReview = new Review(foundReview, foundStars, foundRestaurantId, foundId);
                allReviews.Add(newReview);
            }

            DB.CloseSqlConnection(rdr, conn);

            return allReviews;
        }

        public static Review Find(int reviewId)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE id = @ReviewId;", conn);

            cmd.Parameters.Add(new SqlParameter("@ReviewId", reviewId));
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundId = 0;
            string foundReview = null;
            int foundStars = 0;
            int foundRestaurantId = 0;

            while(rdr.Read())
            {
                foundId = rdr.GetInt32(0);
                foundReview = rdr.GetString(1);
                foundStars = rdr.GetInt32(2);
                foundRestaurantId = rdr.GetInt32(3);
            }

            Review newReview = new Review(foundReview, foundStars, foundRestaurantId, foundId);

            DB.CloseSqlConnection(rdr, conn);

            return newReview;
        }

        public void Update(string review, int stars)
        {
            int targetId = this.GetId();

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE reviews SET review = @Review, stars = @Stars WHERE id = @TargetId", conn);

            cmd.Parameters.Add(new SqlParameter("@Review", review));
            cmd.Parameters.Add(new SqlParameter("@Stars", stars));
            cmd.Parameters.Add(new SqlParameter("@TargetId", targetId));

            cmd.ExecuteNonQuery();

            Review updatedReview = Find(targetId);
            this._review = updatedReview.GetReview();
            this._stars = updatedReview.GetStars();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public void DeleteRestaurantsReviews()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM reviews WHERE restaurant_id = @RestaurantId", conn);

            cmd.Parameters.Add(new SqlParameter("@RestaurantId", this.GetId()));
            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public void DeleteReview()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM reviews WHERE id = @TargetId", conn);

            cmd.Parameters.Add(new SqlParameter("@TargetId", this.GetId()));
            cmd.ExecuteNonQuery();

            if(conn != null)
            {
                conn.Close();
            }
        }

        public int GetId()
        {
            return _id;
        }
        public void SetId(int id)
        {
            _id = id;
        }
        public string GetReview()
        {
            return _review;
        }
        public void SetReview(string review)
        {
            _review = review;
        }
        public int GetStars()
        {
            return _stars;
        }
        public void SetStars(int stars)
        {
            _stars = stars;
        }
        public int GetRestaurantId()
        {
            return _restaurantId;
        }
        public void SetRestaurantId(int restaurantId)
        {
            _restaurantId = restaurantId;
        }

        public static void DeleteAll()
        {
            DB.TableDeleteAll("reviews");
        }
    }
}
