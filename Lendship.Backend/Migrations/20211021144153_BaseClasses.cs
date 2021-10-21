using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class BaseClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClosedGroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructionManual = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: true),
                    Credit = table.Column<int>(type: "int", nullable: true),
                    Deposit = table.Column<int>(type: "int", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisements_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Advertisements_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Availabilites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Availabilites_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClosedGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClosedGroups_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversation_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationAdvertisers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserToId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true),
                    Flexibility = table.Column<int>(type: "int", nullable: false),
                    Reliability = table.Column<int>(type: "int", nullable: false),
                    QualityOfProduct = table.Column<int>(type: "int", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationAdvertisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationAdvertisers_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationAdvertisers_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationAdvertisers_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationLenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserToId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true),
                    Flexibility = table.Column<int>(type: "int", nullable: false),
                    Reliability = table.Column<int>(type: "int", nullable: false),
                    QualityAtReturn = table.Column<int>(type: "int", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationLenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationLenders_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationLenders_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluationLenders_AspNetUsers_UserToId",
                        column: x => x.UserToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdvertisementId = table.Column<int>(type: "int", nullable: true),
                    ReservationState = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admittedByAdvertiser = table.Column<bool>(type: "bit", nullable: false),
                    admittedByLender = table.Column<bool>(type: "bit", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Advertisements_AdvertisementId",
                        column: x => x.AdvertisementId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFromId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_UserFromId",
                        column: x => x.UserFromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClosedGroupId",
                table: "AspNetUsers",
                column: "ClosedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CategoryId",
                table: "Advertisements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilites_AdvertisementId",
                table: "Availabilites",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedGroups_AdvertisementId",
                table: "ClosedGroups",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_AdvertisementId",
                table: "Conversation",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAdvertisers_AdvertisementId",
                table: "EvaluationAdvertisers",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAdvertisers_UserFromId",
                table: "EvaluationAdvertisers",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationAdvertisers_UserToId",
                table: "EvaluationAdvertisers",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationLenders_AdvertisementId",
                table: "EvaluationLenders",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationLenders_UserFromId",
                table: "EvaluationLenders",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationLenders_UserToId",
                table: "EvaluationLenders",
                column: "UserToId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_UserFromId",
                table: "Messages",
                column: "UserFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AdvertisementId",
                table: "Reservations",
                column: "AdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ClosedGroups_ClosedGroupId",
                table: "AspNetUsers",
                column: "ClosedGroupId",
                principalTable: "ClosedGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ClosedGroups_ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Availabilites");

            migrationBuilder.DropTable(
                name: "ClosedGroups");

            migrationBuilder.DropTable(
                name: "EvaluationAdvertisers");

            migrationBuilder.DropTable(
                name: "EvaluationLenders");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClosedGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "AspNetUsers");
        }
    }
}
