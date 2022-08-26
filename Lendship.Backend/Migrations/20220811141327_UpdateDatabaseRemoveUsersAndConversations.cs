using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class UpdateDatabaseRemoveUsersAndConversations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilites_Advertisements_AdvertisementId",
                table: "Availabilites");

            migrationBuilder.DropTable(
                name: "UsersAndConversations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersAndClosedGroups",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "Availabilites",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAndClosedGroups_ClosedGroupId",
                table: "UsersAndClosedGroups",
                column: "ClosedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAndClosedGroups_UserId",
                table: "UsersAndClosedGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilites_Advertisements_AdvertisementId",
                table: "Availabilites",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAndClosedGroups_AspNetUsers_UserId",
                table: "UsersAndClosedGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAndClosedGroups_ClosedGroups_ClosedGroupId",
                table: "UsersAndClosedGroups",
                column: "ClosedGroupId",
                principalTable: "ClosedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilites_Advertisements_AdvertisementId",
                table: "Availabilites");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAndClosedGroups_AspNetUsers_UserId",
                table: "UsersAndClosedGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersAndClosedGroups_ClosedGroups_ClosedGroupId",
                table: "UsersAndClosedGroups");

            migrationBuilder.DropIndex(
                name: "IX_UsersAndClosedGroups_ClosedGroupId",
                table: "UsersAndClosedGroups");

            migrationBuilder.DropIndex(
                name: "IX_UsersAndClosedGroups_UserId",
                table: "UsersAndClosedGroups");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UsersAndClosedGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertisementId",
                table: "Availabilites",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "UsersAndConversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAndConversations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilites_Advertisements_AdvertisementId",
                table: "Availabilites",
                column: "AdvertisementId",
                principalTable: "Advertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
