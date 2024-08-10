using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetProject.Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pet_photo_pet_pet_id",
                table: "pet_photo");

            migrationBuilder.DropPrimaryKey(
                name: "pk_volunteer",
                table: "volunteer");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pet_photo",
                table: "pet_photo");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pet",
                table: "pet");

            migrationBuilder.RenameTable(
                name: "volunteer",
                newName: "volunteers");

            migrationBuilder.RenameTable(
                name: "pet_photo",
                newName: "pet_photos");

            migrationBuilder.RenameTable(
                name: "pet",
                newName: "pets");

            migrationBuilder.RenameIndex(
                name: "ix_pet_photo_pet_id",
                table: "pet_photos",
                newName: "ix_pet_photos_pet_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_volunteers",
                table: "volunteers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pet_photos",
                table: "pet_photos",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pets",
                table: "pets",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_pet_photos_pets_pet_id",
                table: "pet_photos",
                column: "pet_id",
                principalTable: "pets",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pet_photos_pets_pet_id",
                table: "pet_photos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_volunteers",
                table: "volunteers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pets",
                table: "pets");

            migrationBuilder.DropPrimaryKey(
                name: "pk_pet_photos",
                table: "pet_photos");

            migrationBuilder.RenameTable(
                name: "volunteers",
                newName: "volunteer");

            migrationBuilder.RenameTable(
                name: "pets",
                newName: "pet");

            migrationBuilder.RenameTable(
                name: "pet_photos",
                newName: "pet_photo");

            migrationBuilder.RenameIndex(
                name: "ix_pet_photos_pet_id",
                table: "pet_photo",
                newName: "ix_pet_photo_pet_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_volunteer",
                table: "volunteer",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pet",
                table: "pet",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_pet_photo",
                table: "pet_photo",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_pet_photo_pet_pet_id",
                table: "pet_photo",
                column: "pet_id",
                principalTable: "pet",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
