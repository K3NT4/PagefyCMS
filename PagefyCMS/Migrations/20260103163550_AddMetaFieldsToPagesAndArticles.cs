using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagefyCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddMetaFieldsToPagesAndArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Pages",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Pages",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Pages",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Articles",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "Articles",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Articles",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Articles");
        }
    }
}
