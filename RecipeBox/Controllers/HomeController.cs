using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
    public class HomeController : Controller
    {

        private readonly RecipeBoxContext _db;

        public HomeController(RecipeBoxContext db)
        {
            _db = db;
        }

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Results(string results)
        {
            results = results.ToLower();
            List<Recipe> model = _db.Recipes.Include(recipe => recipe.Cuisines).ToList();
            List<Recipe> newModel = new List<Recipe>{};
            for (int i = 0; i < model.Count; i++)
            {
              model[i].Name = model[i].Name.ToLower();
              newModel.Add(model[i]);
            }
            return View("Results", newModel.Where(r => r.Name.Contains(results)).ToList());
        }

        [HttpPost]
        public ActionResult IngredientsResults(string ingredientsresults)
        {
            // results = results.ToLower();
            System.Console.WriteLine(ingredientsresults);
            List<Recipe> model = _db.Recipes.Include(recipe => recipe.Cuisines).ToList();
            List<Recipe> newModel = new List<Recipe>{};
            for (int i = 0; i < model.Count; i++)
            {
              model[i].Ingredients = model[i].Ingredients.ToLower();
              newModel.Add(model[i]);
            }
            System.Console.WriteLine(newModel.Count);
            return View("IngredientsResults", newModel.Where(r => r.Ingredients.Contains(ingredientsresults)).ToList());
        }
    }
}
