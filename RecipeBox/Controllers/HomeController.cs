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
            System.Console.WriteLine(model.Count);
            List<Recipe> newModel = new List<Recipe>{};
            for (int i = 0; i < model.Count; i++)
            {
              model[i].Name = model[i].Name.ToLower();
              newModel.Add(model[i]);
            }

            System.Console.WriteLine(newModel.Count);

            return View("Results", newModel.Where(r => r.Name.Contains(results)).ToList());
        }
    }
}




// li>@Html.ActionLink($"{result.Name}", "Details", "Recipe", new { id = result.RecipeId }) | @result.TotalTime</li>