using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinhocOnline.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSubjectAndTopicCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Topics_TopicCode",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TopicCode",
                table: "Topics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TopicCode",
                table: "Topics",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TopicCode",
                table: "Topics",
                column: "TopicCode",
                unique: true);
        }
    }
}
