using System.Collections.Generic;

namespace RecipeBox.Models
{
  public class Cuisine
    {
        public Cuisine()
        {
            this.Recipes = new HashSet<CuisineRecipe>();
        }

        public int CuisineId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<CuisineRecipe> Recipes { get; set; }
    }
}