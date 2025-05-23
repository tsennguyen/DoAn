﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Laptop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrandAndCategoryModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Brands");
        }
    }
}
