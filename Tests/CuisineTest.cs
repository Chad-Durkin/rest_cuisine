using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantCuisine
{
    public class CuisineTest : IDisposable
    {
        public CuisineTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=restaurantdb_test;Integrated Security=SSPI;";
        }

        // FACTS go here
        [Fact]
        public void Test_CuisineEmptyAtFirst()
        {
            // Arrange
            int result = Cuisine.GetAll().Count;

            // Act

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_IdentityTest_ReturnTrueIfCuisinessAreIdentitical()
        {
             // Arrange, Act
            Cuisine cuisine1 = new Cuisine("Italian");
            Cuisine cuisine2 = new Cuisine("Italian");

            // Assert
            Assert.Equal(cuisine1, cuisine2);
        }

        [Fact]
        public void Save_SavesCuisineInDatabase_ReturnNewId()
        {
            // Arrange
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();

            // Act
            Cuisine retrievedCuisine = Cuisine.GetAll()[0];

            // Assert
            Assert.Equal(testCuisine, retrievedCuisine);
        }

        [Fact]
        public void Find_LocateCuisineInDatabase_ReturnDesiredCuisine()
        {
             // Arrange
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();

            // Act
            Cuisine retrievedCuisine = Cuisine.Find(testCuisine.GetId());

            // Assert
            Assert.Equal(testCuisine, retrievedCuisine);
        }

        // [Fact]
        // public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
        // {
        //       // Arrange
        //     Cuisine testCuisine = new Cuisine("Italian");
        //     testCuisine.Save();
        //
        //     // Act
        //     Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", testCuisine.GetId());
        //
        //     List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant};
        //     List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();
        //
        //     // Assert
        //     Assert.Equal(testRestaurantList, resultRestaurantList);
        // }

        public void Dispose()
        {
            Cuisine.DeleteAll();
        }
    }
}
