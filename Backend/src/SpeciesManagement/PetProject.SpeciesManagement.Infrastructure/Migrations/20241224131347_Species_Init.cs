﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetProject.SpeciesManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Species_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "species");

            migrationBuilder.CreateTable(
                name: "species",
                schema: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "breeds",
                schema: "species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: true),
                    breed_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_species_species_id",
                        column: x => x.species_id,
                        principalSchema: "species",
                        principalTable: "species",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                schema: "species",
                table: "breeds",
                column: "species_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "breeds",
                schema: "species");

            migrationBuilder.DropTable(
                name: "species",
                schema: "species");
        }
    }
}
