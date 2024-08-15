using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetProject.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ReworkDetailsProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "experience",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "pets_adopted",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "pets_found_home_quantity",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "pets_in_treatment",
                table: "volunteers");

            migrationBuilder.DropColumn(
                name: "requisites",
                table: "pets");

            migrationBuilder.AddColumn<string>(
                name: "details",
                table: "pets",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "details",
                table: "pets");

            migrationBuilder.AddColumn<int>(
                name: "experience",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pets_adopted",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pets_found_home_quantity",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pets_in_treatment",
                table: "volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "requisites",
                table: "pets",
                type: "jsonb",
                nullable: true);
        }
    }
}
