using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cloud_FinalTwo.Data.Migrations
{
    /// <inheritdoc />
    public partial class tenth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_Number = table.Column<int>(type: "int", nullable: false),
                    emailOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ordered_Products = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_total = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_Qaunt = table.Column<int>(type: "int", nullable: true),
                    isProcessed = table.Column<bool>(type: "bit", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.order_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
