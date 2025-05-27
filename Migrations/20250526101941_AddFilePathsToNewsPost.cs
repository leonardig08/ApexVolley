using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApexVolley.Migrations
{
    /// <inheritdoc />
    public partial class AddFilePathsToNewsPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalImagePaths",
                table: "NewsPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPaths",
                table: "NewsPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainImagePath",
                table: "NewsPost",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalImagePaths",
                table: "NewsPost");

            migrationBuilder.DropColumn(
                name: "AttachmentPaths",
                table: "NewsPost");

            migrationBuilder.DropColumn(
                name: "MainImagePath",
                table: "NewsPost");
        }
    }
}
