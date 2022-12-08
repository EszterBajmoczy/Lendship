using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class FixNaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "admittedByLender",
                table: "Reservations",
                newName: "AdmittedByLender");

            migrationBuilder.RenameColumn(
                name: "admittedByAdvertiser",
                table: "Reservations",
                newName: "AdmittedByAdvertiser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdmittedByLender",
                table: "Reservations",
                newName: "admittedByLender");

            migrationBuilder.RenameColumn(
                name: "AdmittedByAdvertiser",
                table: "Reservations",
                newName: "admittedByAdvertiser");
        }
    }
}
