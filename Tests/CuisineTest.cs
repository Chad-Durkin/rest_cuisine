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
        public void Save_PreventsDuplicate_ListOfOne()
        {
            // Arrange
            Cuisine cuisine1 = new Cuisine("Italian");
            Cuisine cuisine2 = new Cuisine("Italian");
            List<Cuisine> testList = new List<Cuisine> {cuisine1};

            // Act
            cuisine1.Save();
            cuisine2.Save();

            // Assert
            Assert.Equal(testList.Count, Cuisine.GetAll().Count);

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

        [Fact]
        public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
        {
              // Arrange
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();
            Cuisine testCuisine2 = new Cuisine("Mexican");
            testCuisine.Save();

            // Act
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", testCuisine.GetId());
            Restaurant secondRestaurant = new Restaurant("El Taco Bueno", "7th Street", "530-899-5555", testCuisine2.GetId());
            Restaurant thirdRestaurant = new Restaurant("Taqueria", "10th Street", "435-899-5555", testCuisine2.GetId());
            firstRestaurant.Save();
            secondRestaurant.Save();
            thirdRestaurant.Save();

            List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant};
            List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

            // Assert
            Assert.Equal(testRestaurantList, resultRestaurantList);
        }

        [Fact]
        public void Test_UpdateCuisineName()
        {
            // Arrange
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();

            // Act
            testCuisine.Update("Eastern Italian");

            // Assert
            Assert.Equal("Eastern Italian", testCuisine.GetName());
        }

        [Fact]
        public void Cuisine_Delete_RemoveCuisineFromDatabase()
        {
            // Arrange
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();
            Cuisine testCuisine2 = new Cuisine("Mexican");
            testCuisine2.Save();

            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", testCuisine.GetId());
            Restaurant secondRestaurant = new Restaurant("El Taco Bueno", "7th Street", "530-899-5555", testCuisine2.GetId());
            Restaurant thirdRestaurant = new Restaurant("Taqueria", "10th Street", "435-899-5555", testCuisine2.GetId());
            firstRestaurant.Save();
            secondRestaurant.Save();
            thirdRestaurant.Save();

            List<Cuisine> cuisineTestList = new List<Cuisine> {testCuisine2};
            List<Restaurant> restaurantTestList = new List<Restaurant> {secondRestaurant, thirdRestaurant};


            // Act
            testCuisine.DeleteCuisineAndRestaurants();
            List<Cuisine> cuisineResultList = Cuisine.GetAll();
            List<Restaurant> restaurantResultList = Restaurant.GetAll();

            // Assert
            Assert.Equal(cuisineTestList[0].GetName(), cuisineResultList[0].GetName());
            Assert.Equal(restaurantTestList, restaurantResultList);
        }

        public void Dispose()
        {
            Cuisine.DeleteAll();
            Restaurant.DeleteAll();
        }
    }
}
