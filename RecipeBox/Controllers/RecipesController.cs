using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    public async Task<ActionResult> Index()
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var userRecipes = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id);
        return View(userRecipes);
    }

    public ActionResult Create()
    {
      ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Recipe recipe, int CuisineId)
    {
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        recipe.User = currentUser;
        _db.Recipes.Add(recipe);
        if (CuisineId != 0)
        {
            _db.CuisineRecipes.Add(new CuisineRecipe() { CuisineId = CuisineId, RecipeId = recipe.RecipeId });
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisRecipe = _db.Recipes
        .Include(Recipe => Recipe.Cuisines)
        .ThenInclude(join => join.Cuisine)
        .FirstOrDefault(Recipe => Recipe.RecipeId == id);
      return View(thisRecipe);
    }

    public ActionResult Edit(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
        return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult Edit(Recipe recipe, int CuisineId)
    {
        if (CuisineId != 0)
        {
            _db.CuisineRecipes.Add(new CuisineRecipe() { CuisineId = CuisineId, RecipeId = recipe.RecipeId });
        }
        _db.Entry(recipe).State = EntityState.Modified;
        _db.SaveChanges();
        int id = recipe.RecipeId;
        return RedirectToAction("Details", "Recipes", new { id });
    }

    public ActionResult AddCuisine(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        ViewBag.CuisineId = new SelectList(_db.Cuisines, "CuisineId", "Type");
        return View(thisRecipe);
    }

    [HttpPost]
    public ActionResult AddCuisine(Recipe recipe, int CuisineId)
    {
        if (CuisineId != 0)
        {
        _db.CuisineRecipes.Add(new CuisineRecipe() { CuisineId = CuisineId, RecipeId = recipe.RecipeId });
        }
        _db.SaveChanges();
        int id = recipe.RecipeId;
        return RedirectToAction("Details", "Recipes", new { id });
    }

    public ActionResult Delete(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        return View(thisRecipe);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        var thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
        _db.Recipes.Remove(thisRecipe);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteCuisine(int joinId)
    {
        var joinEntry = _db.CuisineRecipes.FirstOrDefault(entry => entry.CuisineRecipeId == joinId);
        _db.CuisineRecipes.Remove(joinEntry);
        _db.SaveChanges();
        int id = joinEntry.RecipeId;
        return RedirectToAction("Details", "Recipes", new { id });
    }
  }
}