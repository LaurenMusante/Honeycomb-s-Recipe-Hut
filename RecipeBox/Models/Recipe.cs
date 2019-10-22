using System.Collections.Generic;

namespace RecipeBox.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Cuisines = new HashSet<CuisineRecipe>();
        }

        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string TotalTime { get; set; }
        public virtual ApplicationUser User { get; set; }

        public ICollection<CuisineRecipe> Cuisines { get;}
    }
}