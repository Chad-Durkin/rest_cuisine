using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace RestaurantCuisine
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => {
                List<Cuisine> allCuisines = Cuisine.GetAll();
                return View["index.cshtml", allCuisines];
            };
            Get["/cuisine/form"] = _ => {
                return View["cuisine_form.cshtml"];
            };
            Get["/restaurant/form"] = _ => {
                return View["restaurant_form.cshtml"];
            };
            Get["/cuisines"] = _ => {
                List<Cuisine> allCuisines = Cuisine.GetAll();
                return View["index.cshtml", allCuisines];
            };
            Post["/cuisines"] = _ => {
                Cuisine newCuisine = new Cuisine(Request.Form["cuisine"]);
                newCuisine.Save();
                List<Cuisine> allCuisines = Cuisine.GetAll();
                return View["index.cshtml", allCuisines];
            };
            Get["/restaurants"] = _ => {
                List<Cuisine> allCuisines = Cuisine.GetAll();
                return View["index.cshtml", allCuisines];
            };
            Post["/restaurants"] = _ => {
                if(Cuisine.IsNewCuisine(Request.Form["restaurant-cuisine"]) != -1)
                {
                    Restaurant newRestaurant = new Restaurant(
                    Request.Form["restaurant-name"],
                    Request.Form["restaurant-address"],
                    Request.Form["restaurant-phonenumber"],
                    Cuisine.IsNewCuisine(Request.Form["restaurant-cuisine"]));
                    newRestaurant.Save();
                }
                else
                {
                    Cuisine newCuisine = new Cuisine(Request.Form["restaurant-cuisine"]);
                    newCuisine.Save();
                    Restaurant newRestaurant = new Restaurant(
                    Request.Form["restaurant-name"],
                    Request.Form["restaurant-address"],
                    Request.Form["restaurant-phonenumber"],
                    Cuisine.IsNewCuisine(Request.Form["restaurant-cuisine"]));
                    newRestaurant.Save();
                }

                List<Cuisine> allCuisines = Cuisine.GetAll();
                return View["index.cshtml", allCuisines];
            };
            Get["/cuisine/{id}"] = parameters =>
            {
                Cuisine newCuisine = Cuisine.Find(parameters.id);
                return View["cuisine.cshtml", newCuisine];
            };
            Post["/delete/cuisine/{id}"] = parameters =>
            {
                Cuisine targetCuisine = Cuisine.Find(parameters.id);
                targetCuisine.DeleteCuisineAndRestaurants();
                return View["index.cshtml", Cuisine.GetAll()];
            };
            Post["/delete/restaurant/{id}"] = parameters =>
            {
                Restaurant targetRestaurant = Restaurant.Find(parameters.id);
                int cuisineId = targetRestaurant.GetCuisineId();
                Restaurant.Delete(parameters.id);
                return View["cuisine.cshtml", Cuisine.Find(cuisineId)];
            };
            Get["/review/restaurant/{id}"] = parameters =>
            {
                Dictionary<string, object> model = new Dictionary<string, object> {};
                Restaurant targetRestaurant = Restaurant.Find(parameters.id);
                List<Review> allReviews = Review.FindAll(parameters.id);
                model.Add("restaurant", targetRestaurant);
                model.Add("reviews", allReviews);
                return View["restaurant.cshtml", model];
            };
            Post["/review/restaurant/{id}"] = parameters =>
            {
                Review newReview = new Review(Request.Form["review"], Request.Form["stars"], parameters.id);
                newReview.Save();
                Dictionary<string, object> model = new Dictionary<string, object> {};
                Restaurant targetRestaurant = Restaurant.Find(parameters.id);
                List<Review> allReviews = Review.FindAll(parameters.id);
                model.Add("restaurant", targetRestaurant);
                model.Add("reviews", allReviews);
                return View["restaurant.cshtml", model];
            };
        }
    }
}
