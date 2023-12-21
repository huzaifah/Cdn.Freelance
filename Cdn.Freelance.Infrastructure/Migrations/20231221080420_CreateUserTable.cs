using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cdn.Freelance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "freelance");

            migrationBuilder.CreateSequence(
                name: "userseq",
                schema: "freelance",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "users",
                schema: "freelance",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    identity_guid = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    user_name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    email_address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    hobby = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified_by = table.Column<string>(type: "text", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "skillset",
                schema: "freelance",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    skill = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_skillset", x => x.id);
                    table.ForeignKey(
                        name: "fk_skillset_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "freelance",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_skillset_user_id",
                schema: "freelance",
                table: "skillset",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_identity_guid",
                schema: "freelance",
                table: "users",
                column: "identity_guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "skillset",
                schema: "freelance");

            migrationBuilder.DropTable(
                name: "users",
                schema: "freelance");

            migrationBuilder.DropSequence(
                name: "userseq",
                schema: "freelance");
        }
    }
}
