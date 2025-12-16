using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinhocOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentAnalysisTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentExamAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentExamId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    WrongAnswers = table.Column<int>(type: "int", nullable: false),
                    AccuracyPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    AnalysisResultJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExamAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentExamAnalyses_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExamAnalyses_StudentExams_StudentExamId",
                        column: x => x.StudentExamId,
                        principalTable: "StudentExams",
                        principalColumn: "StudentExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExamAnalyses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentTopicPerformances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TopicName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalAttempts = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    AccuracyPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LastAttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTopicPerformances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTopicPerformances_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAnalyses_ExamId",
                table: "StudentExamAnalyses",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAnalyses_StudentExamId",
                table: "StudentExamAnalyses",
                column: "StudentExamId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAnalyses_UserId",
                table: "StudentExamAnalyses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTopicPerformances_UserId",
                table: "StudentTopicPerformances",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExamAnalyses");

            migrationBuilder.DropTable(
                name: "StudentTopicPerformances");
        }
    }
}
