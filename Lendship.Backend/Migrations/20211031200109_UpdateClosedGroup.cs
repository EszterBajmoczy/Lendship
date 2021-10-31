using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class UpdateClosedGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ClosedGroups_ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClosedGroups_Advertisements_AdvertisementId",
                table: "ClosedGroups");

            migrationBuilder.DropIndex(
                name: "IX_ClosedGroups_AdvertisementId",
                table: "ClosedGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdvertisementId",
                table: "ClosedGroups");

            migrationBuilder.DropColumn(
                name: "ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AdvertismentId",
                table: "ClosedGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserIds",
                table: "ClosedGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertismentId",
                table: "ClosedGroups");

            migrationBuilder.DropColumn(
                name: "UserIds",
                table: "ClosedGroups");

            migrationBuilder.AddColumn<int>(
                name: "AdvertisementId",
                table: "ClosedGroups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClosedGroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClosedGroups_AdvertisementId",
                table: "ClosedGroups",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClosedGroupId",
                table: "AspNetUsers",
                column: "ClosedGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ClosedGroups_ClosedGroupId",
                table: "AspNetUsers",
                column: "ClosedGroupId",
                principalTable: "ClosedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedGroups_Advertisements_AdvertisementId",
                table: "ClosedGroups",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
