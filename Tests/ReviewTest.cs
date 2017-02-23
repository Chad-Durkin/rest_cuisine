using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantCuisine
{
    public class ReviewTest : IDisposable
    {
        public ReviewTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurantdb_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_ReviewEmptyFirst()
        {
            // Arrange
            int result = Review.GetAll().Count;
            // Act
            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Review_Compare_ReturnTrueIfIdentical()
        {
            // Arrange, Act
            Review firstReview = new Review("Their pizza was marvelous", 4, 0);
            Review secondReview = new Review("Their pizza was marvelous", 4, 0);

            // Assert
            Assert.Equal(firstReview, secondReview);
        }

        [Fact]
        public void Test_Review_Save_CheckIfSavedReview()
        {
            // Arrange
            Review firstReview = new Review("Their pizza was marvelous", 4, 0);
            firstReview.Save();

            // Act
            Review result = Review.GetAll()[0];

            // Assert
            Assert.Equal(firstReview, result);
        }

        [Fact]
        public void Test_Find_FindsReviewsByRestaurantIdInDatabase()
        {
            // Arrange
            Restaurant testRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            testRestaurant.Save();
            Review testReview = new Review("Their pizza was marvelous", 4, testRestaurant.GetId());
            testReview.Save();
            Review testReview1 = new Review("Their pizza was bland", 2, testRestaurant.GetId());
            testReview1.Save();
            Review testReview2 = new Review("Best pizza ever", 5, testRestaurant.GetId());
            testReview2.Save();
            Review testReview3 = new Review("Great Spaghetti", 3, 0);
            testReview3.Save();

            // Act
            List<Review> checkReviews = new List<Review> {testReview, testReview1, testReview2};
            List<Review> foundReviews = Review.FindAll(testRestaurant.GetId());

            // Assert
            Assert.Equal(checkReviews, foundReviews);
        }

        [Fact]
        public void Test_Review_Update_ChangeReviewInfo()
        {
            // Arrange
            Restaurant testRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            testRestaurant.Save();
            Review testReview = new Review("Their pizza was marvelous", 4, testRestaurant.GetId());
            testReview.Save();
            Review testReview1 = new Review("First time was great, second time their pizza was bland", 2, testRestaurant.GetId());
            testReview1.Save();

            // Act
            testReview.Update("First time was great, second time their pizza was bland", 2);
            // testReview.SetId(testRestaurant.GetId());
            // testReview1.SetId(testRestaurant.GetId());

            // Assert
            Assert.Equal(testReview.GetReview(), testReview1.GetReview());
        }

        [Fact]
        public void Test_Review_DeleteFromDatabaseByReviewId()
        {
            // Arrange
            Restaurant testRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            testRestaurant.Save();
            Review testReview = new Review("Their pizza was marvelous", 4, testRestaurant.GetId());
            testReview.Save();
            Review testReview1 = new Review("First time was great, second time their pizza was bland", 2, testRestaurant.GetId());
            testReview1.Save();

            // Act
            testReview.DeleteReview();

            // Assert
            Assert.Equal(1, Restaurant.GetAll().Count);
        }


        public void Dispose()
        {
            Cuisine.DeleteAll();
            Restaurant.DeleteAll();
            Review.DeleteAll();
        }
    }
}
