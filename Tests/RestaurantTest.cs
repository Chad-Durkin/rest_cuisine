using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantCuisine
{
    public class RestaurantTest : IDisposable
    {
        public RestaurantTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurantdb_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void Test_DatabaseEmptyAtFirst()
        {
            // Arranace, Act
            int result = Restaurant.GetAll().Count;

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Restaurant_Compare_ReturnTrueIfIdentical()
        {
            // Arrange, Act
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            Restaurant secondRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);

            // Assert
            Assert.Equal(firstRestaurant, secondRestaurant);
        }

        [Fact]
        public void Restaurant_Save_AlterIdOfSavedRestaurant()
        {
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            firstRestaurant.Save();

            // Act
            Restaurant result = Restaurant.GetAll()[0];

            // Assert
            Assert.Equal(firstRestaurant, result);
        }

        [Fact]
        public void Test_Find_FindsRestaurantInDatabase()
        {
            // Arrange
            Restaurant testRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            testRestaurant.Save();

            // Act
            Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetId());

            // Assert
            Assert.Equal(testRestaurant, foundRestaurant);
        }

        public void Dispose()
        {
            Restaurant.DeleteAll();
        }
    }
}
