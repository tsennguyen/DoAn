using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Laptop.Migrations
{
    /// <inheritdoc />
    public partial class fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "UserBehaviors");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserBehaviors");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "UserBehaviors");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "UserBehaviors");

            migrationBuilder.AlterColumn<string>(
                name: "BehaviorType",
                table: "UserBehaviors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ResultCode",
                table: "MomoInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultCode",
                table: "MomoInfos");

            migrationBuilder.AlterColumn<int>(
                name: "BehaviorType",
                table: "UserBehaviors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "UserBehaviors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "UserBehaviors",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "UserBehaviors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "UserBehaviors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
