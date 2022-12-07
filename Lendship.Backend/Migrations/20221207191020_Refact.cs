using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class Refact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_evaluationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "evaluationId",
                table: "AspNetUsers",
                newName: "EvaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_evaluationId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_EvaluationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_EvaluationId",
                table: "AspNetUsers",
                column: "EvaluationId",
                principalTable: "EvaluationComputed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_EvaluationId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EvaluationId",
                table: "AspNetUsers",
                newName: "evaluationId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_EvaluationId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_evaluationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_evaluationId",
                table: "AspNetUsers",
                column: "evaluationId",
                principalTable: "EvaluationComputed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
