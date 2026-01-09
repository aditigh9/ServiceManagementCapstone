using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApi.Migrations
{
    public partial class AddTechnicianPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // NO-OP: Price column already removed from ServiceCategories
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceCategories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
