using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetProject.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunterDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requisites",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "social_networks",
                table: "volunteers");

            migrationBuilder.AddColumn<string>(
                name: "details",
                table: "volunteers",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "details",
                table: "volunteers");

            migrationBuilder.AddColumn<string>(
                name: "requisites",
                table: "volunteers",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "social_networks",
                table: "volunteers",
                type: "jsonb",
                nullable: true);
        }
    }
}
