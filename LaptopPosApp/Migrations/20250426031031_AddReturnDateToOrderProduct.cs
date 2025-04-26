using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopPosApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnDateToOrderProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReturnDate",
                table: "OrderProduct",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "OrderProduct");
        }
    }
}
