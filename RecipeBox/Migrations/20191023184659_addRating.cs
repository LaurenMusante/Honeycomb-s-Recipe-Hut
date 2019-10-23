using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipeBox.Migrations
{
    public partial class addRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Recipes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Recipes");
        }
    }
}
