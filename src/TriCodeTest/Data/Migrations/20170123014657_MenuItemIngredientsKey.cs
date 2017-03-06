using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TriCodeTest.Data.Migrations
{
    public partial class MenuItemIngredientsKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients",
                columns: new[] { "MenuItemId", "IngredientId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItemIngredients",
                table: "MenuItemIngredients",
                column: "MenuItemId");
        }
    }
}
