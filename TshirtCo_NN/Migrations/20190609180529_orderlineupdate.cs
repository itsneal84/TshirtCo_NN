using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TshirtCo_NN.Migrations
{
    public partial class orderlineupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateShipped",
                table: "OrderLines",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Dispatched",
                table: "OrderLines",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateShipped",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "Dispatched",
                table: "OrderLines");
        }
    }
}
