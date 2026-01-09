using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceManagementApi.Migrations
{
    public partial class AddTechnicians : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Technicians",
                newName: "IsActive");

            
            migrationBuilder.AddColumn<string>(
                name: "Skill",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skill",
                table: "Technicians");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Technicians",
                newName: "IsAvailable");
        }
    }
}
