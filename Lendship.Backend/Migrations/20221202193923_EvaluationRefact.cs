using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class EvaluationRefact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationAdvertisers_Advertisements_AdvertisementId",
                table: "EvaluationAdvertisers");

            migrationBuilder.DropForeignKey(
                name: "FK_EvaluationLenders_Advertisements_AdvertisementId",
                table: "EvaluationLenders");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationLenders_AdvertisementId",
                table: "EvaluationLenders");

            migrationBuilder.DropIndex(
                name: "IX_EvaluationAdvertisers_AdvertisementId",
                table: "EvaluationAdvertisers");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "EvaluationLenders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "EvaluationAdvertisers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "EvaluationLenders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "EvaluationAdvertisers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationLenders_AdvertisementId",
                table: "EvaluationLenders",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAdvertisers_AdvertisementId",
                table: "EvaluationAdvertisers",
                column: "AdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationAdvertisers_Advertisements_AdvertisementId",
                table: "EvaluationAdvertisers",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EvaluationLenders_Advertisements_AdvertisementId",
                table: "EvaluationLenders",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
