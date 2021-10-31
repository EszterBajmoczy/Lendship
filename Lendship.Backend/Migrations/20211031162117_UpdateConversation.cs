using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class UpdateConversation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserIds",
                table: "Conversation",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIds",
                table: "Conversation");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Conversation_ConversationId",
                table: "AspNetUsers",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
