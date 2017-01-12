using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TriCodeTest.Data.Migrations
{
    public partial class OrderModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<double>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedAddOn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedAddOn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderedIngredient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Option = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedIngredient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderedItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Size = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderedItemAddOns",
                columns: table => new
                {
                    OrderedItemId = table.Column<int>(nullable: false),
                    OrderedAddOnId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedItemAddOns", x => x.OrderedItemId);
                    table.ForeignKey(
                        name: "FK_OrderedItemAddOns_OrderedAddOn_OrderedAddOnId",
                        column: x => x.OrderedAddOnId,
                        principalTable: "OrderedAddOn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedItemAddOns_OrderedItem_OrderedItemId",
                        column: x => x.OrderedItemId,
                        principalTable: "OrderedItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedItemIngredients",
                columns: table => new
                {
                    OrderedItemId = table.Column<int>(nullable: false),
                    OrderedIngredientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedItemIngredients", x => x.OrderedItemId);
                    table.ForeignKey(
                        name: "FK_OrderedItemIngredients_OrderedIngredient_OrderedIngredientId",
                        column: x => x.OrderedIngredientId,
                        principalTable: "OrderedIngredient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedItemIngredients_OrderedItem_OrderedItemId",
                        column: x => x.OrderedItemId,
                        principalTable: "OrderedItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderOrderedItems",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false),
                    OrderedItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOrderedItems", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderOrderedItems_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderOrderedItems_OrderedItem_OrderedItemId",
                        column: x => x.OrderedItemId,
                        principalTable: "OrderedItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemAddOns_OrderedAddOnId",
                table: "OrderedItemAddOns",
                column: "OrderedAddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemAddOns_OrderedItemId",
                table: "OrderedItemAddOns",
                column: "OrderedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemIngredients_OrderedIngredientId",
                table: "OrderedItemIngredients",
                column: "OrderedIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItemIngredients_OrderedItemId",
                table: "OrderedItemIngredients",
                column: "OrderedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrderedItems_OrderId",
                table: "OrderOrderedItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderOrderedItems_OrderedItemId",
                table: "OrderOrderedItems",
                column: "OrderedItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedItemAddOns");

            migrationBuilder.DropTable(
                name: "OrderedItemIngredients");

            migrationBuilder.DropTable(
                name: "OrderOrderedItems");

            migrationBuilder.DropTable(
                name: "OrderedAddOn");

            migrationBuilder.DropTable(
                name: "OrderedIngredient");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "OrderedItem");
        }
    }
}
