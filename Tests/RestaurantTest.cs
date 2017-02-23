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
        public void Restaurant_SearchByName_Return()
        {
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            Restaurant secondRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 2);
            Restaurant thirdRestaurant = new Restaurant("DoughBoys", "5th Street", "530-816-9999", 0);
            firstRestaurant.Save();
            secondRestaurant.Save();
            thirdRestaurant.Save();
            List<Restaurant> testList = new List<Restaurant>{firstRestaurant, secondRestaurant};

            // Act
            List<Restaurant> resultList = Restaurant.SearchByName("Piz");

            // Assert
            Assert.Equal(testList, resultList);
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

        [Fact]
        public void Cuisine_IsNewCuisine_ReturnsNegOneOnNothing()
        {
            int cuisineId = Cuisine.IsNewCuisine("Italian");

            Assert.Equal(-1, cuisineId);
        }

        [Fact]
        public void Test_Restaurant_Update_ChangeRestaurantInfo()
        {
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 0);
            firstRestaurant.Save();
            Restaurant secondRestaurant = new Restaurant("New Pizza Place", "7th Street", "530-777-4343", 0);
            secondRestaurant.Save();

            // Act
            firstRestaurant.Update("New Pizza Place", "7th Street", "530-777-4343");
            firstRestaurant.SetId(0);
            secondRestaurant.SetId(0);

            // Assert
            Assert.Equal(firstRestaurant, secondRestaurant);
        }

        [Fact]
        public void Test_Restaurant_Update_ChangeRestaurantCuisine()
        {
            Cuisine testCuisine = new Cuisine("Italian");
            testCuisine.Save();
            Cuisine testCuisine2 = new Cuisine("Sicilian");
            testCuisine2.Save();
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", testCuisine.GetId());
            firstRestaurant.Save();
            Restaurant secondRestaurant = new Restaurant("New Pizza Place", "7th Street", "530-777-4343", testCuisine2.GetId());
            secondRestaurant.Save();

            // Act
            firstRestaurant.Update("New Pizza Place", "7th Street", "530-777-4343", "Sicilian");
            firstRestaurant.SetId(0);
            secondRestaurant.SetId(0);

            // Assert
            Assert.Equal(Restaurant.Find(firstRestaurant.GetId()), Restaurant.Find(secondRestaurant.GetId()));

        }

        [Fact]
        public void Test_Restaurant_Delete_RemoveRestaurantFromDatabase()
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
            Restaurant.Delete(firstRestaurant.GetId());
            List<Restaurant> restaurantResultList = Restaurant.GetAll();

            // Assert
            Assert.Equal(restaurantTestList, restaurantResultList);
        }
        [Fact]
        public void Restaurant_IsDuplicate_ReturnsNegativeOneIfDupe()
        {
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 5);
            firstRestaurant.Save();
            Restaurant secondRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 4);

            List<Restaurant> testList = new List<Restaurant>{firstRestaurant, secondRestaurant};

            // Act
            int result = secondRestaurant.IsNewRestaurant();
            if (result == -1)
            {
                secondRestaurant.Save();
            }

            // Assert
            Assert.Equal(testList, Restaurant.GetAll());
        }
        [Fact]
        public void Restaurant_Save_DoesntSaveExactDuplicates()
        {
            // Arrange
            Restaurant firstRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 5);
            firstRestaurant.Save();
            Restaurant secondRestaurant = new Restaurant("Pizza Factory", "5th Street", "530-816-9999", 5);

            List<Restaurant> testList = new List<Restaurant>{firstRestaurant};

            // Act
            secondRestaurant.Save();

            // Assert
            Assert.Equal(testList, Restaurant.GetAll());
        }

        public void Dispose()
        {
            Restaurant.DeleteAll();
            Cuisine.DeleteAll();
        }
    }
}
