using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class AddEvaluationSummaries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdvertiserFlexibility",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AdvertiserQualityOfProduct",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AdvertiserReliability",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LenderFlexibility",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LenderQualityQualityAtReturn",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LenderReliability",
                table: "AspNetUsers",
                type: "decimal(9,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertiserFlexibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdvertiserQualityOfProduct",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdvertiserReliability",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderFlexibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderQualityQualityAtReturn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderReliability",
                table: "AspNetUsers");
        }
    }
}
