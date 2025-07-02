using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagefyCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddShowInMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInMenu",
                table: "Pages",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInMenu",
                table: "Pages");
        }
    }
}
