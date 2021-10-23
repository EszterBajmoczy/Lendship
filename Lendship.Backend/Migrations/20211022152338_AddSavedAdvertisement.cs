using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class AddSavedAdvertisement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SavedAdvertisementId",
                table: "Advertisements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SavedAdvertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedAdvertisements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_SavedAdvertisementId",
                table: "Advertisements",
                column: "SavedAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedAdvertisements_UserId",
                table: "SavedAdvertisements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisements_SavedAdvertisements_SavedAdvertisementId",
                table: "Advertisements",
                column: "SavedAdvertisementId",
                principalTable: "SavedAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisements_SavedAdvertisements_SavedAdvertisementId",
                table: "Advertisements");

            migrationBuilder.DropTable(
                name: "SavedAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_Advertisements_SavedAdvertisementId",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "SavedAdvertisementId",
                table: "Advertisements");
        }
    }
}
