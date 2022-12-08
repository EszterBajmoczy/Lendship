using Microsoft.EntityFrameworkCore.Migrations;

namespace Lendship.Backend.Migrations
{
    public partial class RefactEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertiserFlexibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdvertiserQualityOfProduct",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdvertiserReliability",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsAdvertiser",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsAdvertiserCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsLender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EvaluationAsLenderCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderFlexibility",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderQualityAtReturn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LenderReliability",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "evaluationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EvaluationComputed",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EvaluationAsAdvertiser = table.Column<double>(type: "float", nullable: false),
                    EvaluationAsAdvertiserCount = table.Column<int>(type: "int", nullable: false),
                    AdvertiserFlexibility = table.Column<double>(type: "float", nullable: false),
                    AdvertiserReliability = table.Column<double>(type: "float", nullable: false),
                    AdvertiserQualityOfProduct = table.Column<double>(type: "float", nullable: false),
                    EvaluationAsLender = table.Column<double>(type: "float", nullable: false),
                    EvaluationAsLenderCount = table.Column<int>(type: "int", nullable: false),
                    LenderFlexibility = table.Column<double>(type: "float", nullable: false),
                    LenderReliability = table.Column<double>(type: "float", nullable: false),
                    LenderQualityAtReturn = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationComputed", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_evaluationId",
                table: "AspNetUsers",
                column: "evaluationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_evaluationId",
                table: "AspNetUsers",
                column: "evaluationId",
                principalTable: "EvaluationComputed",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EvaluationComputed_evaluationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EvaluationComputed");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_evaluationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "evaluationId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "AdvertiserFlexibility",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AdvertiserQualityOfProduct",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AdvertiserReliability",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EvaluationAsAdvertiser",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsAdvertiserCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "EvaluationAsLender",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "EvaluationAsLenderCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "LenderFlexibility",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LenderQualityAtReturn",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LenderReliability",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
