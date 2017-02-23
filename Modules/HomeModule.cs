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
                return View["index.cshtml", newCuisine];
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
        }
    }
}
