using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class FixPrivateUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAndAdvertisement_Advertisements_AdvertisementId",
                table: "UsersAndAdvertisement");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAndAdvertisement_AspNetUsers_UserId",
                table: "UsersAndAdvertisement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersAndAdvertisement",
                table: "UsersAndAdvertisement");

            migrationBuilder.RenameTable(
                name: "UsersAndAdvertisement",
                newName: "PrivateUsers");

            migrationBuilder.RenameIndex(
                name: "IX_UsersAndAdvertisement_UserId",
                table: "PrivateUsers",
                newName: "IX_PrivateUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersAndAdvertisement_AdvertisementId",
                table: "PrivateUsers",
                newName: "IX_PrivateUsers_AdvertisementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrivateUsers",
                table: "PrivateUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateUsers_Advertisements_AdvertisementId",
                table: "PrivateUsers",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateUsers_AspNetUsers_UserId",
                table: "PrivateUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateUsers_Advertisements_AdvertisementId",
                table: "PrivateUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateUsers_AspNetUsers_UserId",
                table: "PrivateUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrivateUsers",
                table: "PrivateUsers");

            migrationBuilder.RenameTable(
                name: "PrivateUsers",
                newName: "UsersAndAdvertisement");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateUsers_UserId",
                table: "UsersAndAdvertisement",
                newName: "IX_UsersAndAdvertisement_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PrivateUsers_AdvertisementId",
                table: "UsersAndAdvertisement",
                newName: "IX_UsersAndAdvertisement_AdvertisementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersAndAdvertisement",
                table: "UsersAndAdvertisement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAndAdvertisement_Advertisements_AdvertisementId",
                table: "UsersAndAdvertisement",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAndAdvertisement_AspNetUsers_UserId",
                table: "UsersAndAdvertisement",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
