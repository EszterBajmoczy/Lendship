using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class RefactMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversation_conversationId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Messages",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "conversationId",
                table: "Messages",
                newName: "ConversationId");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Messages",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_conversationId",
                table: "Messages",
                newName: "IX_Messages_ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversation_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversation_ConversationId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Messages",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "Messages",
                newName: "conversationId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                newName: "IX_Messages_conversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversation_conversationId",
                table: "Messages",
                column: "conversationId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
