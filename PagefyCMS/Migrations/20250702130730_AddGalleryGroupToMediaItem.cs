using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagefyCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryGroupToMediaItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GalleryGroup",
                table: "Pages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GalleryGroup",
                table: "MediaLibrary",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GalleryGroup",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "GalleryGroup",
                table: "MediaLibrary");
        }
    }
}
