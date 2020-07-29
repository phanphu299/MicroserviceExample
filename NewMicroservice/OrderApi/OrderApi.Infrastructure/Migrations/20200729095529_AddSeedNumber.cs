using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderApi.Infrastructure.Migrations
{
    public partial class AddSeedNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderState = table.Column<int>(nullable: false),
                    CustomerGuid = table.Column<Guid>(nullable: false),
                    CustomerFullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
