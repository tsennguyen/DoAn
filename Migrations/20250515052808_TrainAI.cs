using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Laptop.Migrations
{
    public partial class TrainAI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Nếu bạn không cần rename, bỏ đoạn này đi:
            // migrationBuilder.RenameColumn(
            //     name: "PaymentMethod",
            //     table: "Orders",
            //     newName: "PaymentMethod");

            // Nếu bạn muốn sửa tên cột từ 'PaymenMethod' sang 'PaymentMethod' thì dùng:
            // migrationBuilder.RenameColumn(
            //     name: "PaymenMethod",
            //     table: "Orders",
            //     newName: "PaymentMethod");

            migrationBuilder.CreateTable(
                name: "UserBehaviors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BehaviorType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBehaviors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBehaviors_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_ProductId",
                table: "UserBehaviors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBehaviors_UserId",
                table: "UserBehaviors",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBehaviors");

            // Nếu bạn bỏ rename ở Up, bỏ luôn rename ở Down
            // migrationBuilder.RenameColumn(
            //     name: "PaymentMethod",
            //     table: "Orders",
            //     newName: "PaymenMethod");
        }
    }
}
