using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PagefyCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveThemeToCmsSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveTheme",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StartpageSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Layout = table.Column<string>(type: "TEXT", nullable: false),
                    SectionsJson = table.Column<string>(type: "TEXT", nullable: false),
                    Sections = table.Column<string>(type: "TEXT", nullable: false),
                    Colors_Background = table.Column<string>(type: "TEXT", nullable: false),
                    Colors_Primary = table.Column<string>(type: "TEXT", nullable: false),
                    Colors_Text = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    PrimaryColor = table.Column<string>(type: "TEXT", nullable: false),
                    TextColor = table.Column<string>(type: "TEXT", nullable: false),
                    HeroBackground = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartpageSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StartpageSettings");

            migrationBuilder.DropColumn(
                name: "ActiveTheme",
                table: "Settings");
        }
    }
}
