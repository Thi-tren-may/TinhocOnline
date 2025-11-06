using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinhocOnline.Migrations
{
    /// <inheritdoc />
    public partial class AddExamTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationTaken",
                table: "Student_Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                table: "Student_Exams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradeLevel",
                table: "Questions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamTypeId",
                table: "Exams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GradeLevel",
                table: "Exams",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                table: "Exams",
                type: "decimal(4,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ShuffleAnswers",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShuffleQuestions",
                table: "Exams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ExamTypes",
                columns: table => new
                {
                    ExamTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DefaultDuration = table.Column<int>(type: "int", nullable: false),
                    DefaultTotalQuestions = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTypes", x => x.ExamTypeId);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Questions_GradeLevel",
                table: "Questions",
                sql: "[GradeLevel] IS NULL OR [GradeLevel] IN ('10', '11', '12')");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamTypeId",
                table: "Exams",
                column: "ExamTypeId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Exams_GradeLevel",
                table: "Exams",
                sql: "[GradeLevel] IS NULL OR [GradeLevel] IN ('10', '11', '12')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Exams_PassingScore",
                table: "Exams",
                sql: "[PassingScore] >= 0 AND [PassingScore] <= 10");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTypes_TypeCode",
                table: "ExamTypes",
                column: "TypeCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamTypes_ExamTypeId",
                table: "Exams",
                column: "ExamTypeId",
                principalTable: "ExamTypes",
                principalColumn: "ExamTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamTypes_ExamTypeId",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "ExamTypes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Questions_GradeLevel",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ExamTypeId",
                table: "Exams");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Exams_GradeLevel",
                table: "Exams");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Exams_PassingScore",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "DurationTaken",
                table: "Student_Exams");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                table: "Student_Exams");

            migrationBuilder.DropColumn(
                name: "GradeLevel",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ExamTypeId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "GradeLevel",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ShuffleAnswers",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ShuffleQuestions",
                table: "Exams");
        }
    }
}
