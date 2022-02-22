using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class UpdateApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsAdvertiser",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsAdvertiserCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsLender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsLenderCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluationAsAdvertiser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsAdvertiserCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsLender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsLenderCount",
                table: "AspNetUsers");
        }
    }
}
