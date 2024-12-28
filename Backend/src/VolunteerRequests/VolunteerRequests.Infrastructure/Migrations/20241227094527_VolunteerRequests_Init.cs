using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerRequests.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VolunteerRequests_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "volunteers_requests");

            migrationBuilder.CreateTable(
                name: "volunteer_requests",
                schema: "volunteers_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    age_experience = table.Column<int>(type: "integer", nullable: false),
                    general_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    social_networks = table.Column<string>(type: "jsonb", nullable: false),
                    request_status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    admin_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    discussion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    rejection_comment = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_requests", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "volunteer_requests",
                schema: "volunteers_requests");
        }
    }
}
