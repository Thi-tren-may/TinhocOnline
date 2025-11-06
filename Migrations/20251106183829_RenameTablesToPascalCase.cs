using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinhocOnline.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Questions_Exams_ExamId",
                table: "Exam_Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Questions_Questions_QuestionId",
                table: "Exam_Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Answers_Answers_AnswerId",
                table: "Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Answers_Questions_QuestionId",
                table: "Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Answers_Student_Exams_StudentExamId",
                table: "Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Exams_Exams_ExamId",
                table: "Student_Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Exams_Users_StudentId",
                table: "Student_Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Exams",
                table: "Student_Exams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Answers",
                table: "Student_Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_Questions",
                table: "Exam_Questions");

            migrationBuilder.RenameTable(
                name: "Student_Exams",
                newName: "StudentExams");

            migrationBuilder.RenameTable(
                name: "Student_Answers",
                newName: "StudentAnswers");

            migrationBuilder.RenameTable(
                name: "Exam_Questions",
                newName: "ExamQuestions");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Exams_StudentId",
                table: "StudentExams",
                newName: "IX_StudentExams_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Exams_ExamId",
                table: "StudentExams",
                newName: "IX_StudentExams_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Answers_StudentExamId",
                table: "StudentAnswers",
                newName: "IX_StudentAnswers_StudentExamId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Answers_QuestionId",
                table: "StudentAnswers",
                newName: "IX_StudentAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Student_Answers_AnswerId",
                table: "StudentAnswers",
                newName: "IX_StudentAnswers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Questions_QuestionId",
                table: "ExamQuestions",
                newName: "IX_ExamQuestions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Questions_ExamId",
                table: "ExamQuestions",
                newName: "IX_ExamQuestions_ExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExams",
                table: "StudentExams",
                column: "StudentExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers",
                column: "StudentAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExamQuestions",
                table: "ExamQuestions",
                column: "ExamQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_StudentExams_StudentExamId",
                table: "StudentAnswers",
                column: "StudentExamId",
                principalTable: "StudentExams",
                principalColumn: "StudentExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExams_Exams_ExamId",
                table: "StudentExams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExams_Users_StudentId",
                table: "StudentExams",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Exams_ExamId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamQuestions_Questions_QuestionId",
                table: "ExamQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Answers_AnswerId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_StudentExams_StudentExamId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExams_Exams_ExamId",
                table: "StudentExams");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExams_Users_StudentId",
                table: "StudentExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExams",
                table: "StudentExams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentAnswers",
                table: "StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExamQuestions",
                table: "ExamQuestions");

            migrationBuilder.RenameTable(
                name: "StudentExams",
                newName: "Student_Exams");

            migrationBuilder.RenameTable(
                name: "StudentAnswers",
                newName: "Student_Answers");

            migrationBuilder.RenameTable(
                name: "ExamQuestions",
                newName: "Exam_Questions");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExams_StudentId",
                table: "Student_Exams",
                newName: "IX_Student_Exams_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExams_ExamId",
                table: "Student_Exams",
                newName: "IX_Student_Exams_ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAnswers_StudentExamId",
                table: "Student_Answers",
                newName: "IX_Student_Answers_StudentExamId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAnswers_QuestionId",
                table: "Student_Answers",
                newName: "IX_Student_Answers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentAnswers_AnswerId",
                table: "Student_Answers",
                newName: "IX_Student_Answers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamQuestions_QuestionId",
                table: "Exam_Questions",
                newName: "IX_Exam_Questions_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamQuestions_ExamId",
                table: "Exam_Questions",
                newName: "IX_Exam_Questions_ExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Exams",
                table: "Student_Exams",
                column: "StudentExamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Answers",
                table: "Student_Answers",
                column: "StudentAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_Questions",
                table: "Exam_Questions",
                column: "ExamQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Questions_Exams_ExamId",
                table: "Exam_Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Questions_Questions_QuestionId",
                table: "Exam_Questions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Answers_Answers_AnswerId",
                table: "Student_Answers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Answers_Questions_QuestionId",
                table: "Student_Answers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Answers_Student_Exams_StudentExamId",
                table: "Student_Answers",
                column: "StudentExamId",
                principalTable: "Student_Exams",
                principalColumn: "StudentExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Exams_Exams_ExamId",
                table: "Student_Exams",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Exams_Users_StudentId",
                table: "Student_Exams",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
