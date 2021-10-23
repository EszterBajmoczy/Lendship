using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class ModifySavedAdvertisement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_SavedAdvertisements_SavedAdvertisementId",
                table: "Advertisements");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedAdvertisements_AspNetUsers_UserId",
                table: "SavedAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_SavedAdvertisements_UserId",
                table: "SavedAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_SavedAdvertisementId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "SavedAdvertisementId",
                table: "Advertisements");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SavedAdvertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementId",
                table: "SavedAdvertisements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "SavedAdvertisements");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SavedAdvertisements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SavedAdvertisementId",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedAdvertisements_UserId",
                table: "SavedAdvertisements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_SavedAdvertisementId",
                table: "Advertisements",
                column: "SavedAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_SavedAdvertisements_SavedAdvertisementId",
                table: "Advertisements",
                column: "SavedAdvertisementId",
                principalTable: "SavedAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedAdvertisements_AspNetUsers_UserId",
                table: "SavedAdvertisements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
