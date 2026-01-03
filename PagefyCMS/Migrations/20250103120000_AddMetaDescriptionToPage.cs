using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagefyCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddMetaDescriptionToPage : Migration
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
                defaultValue: new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc));
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
        }
    }
}
