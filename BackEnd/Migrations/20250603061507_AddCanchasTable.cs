using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaReservasApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCanchasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "canchas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoDeporte = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PrecioPorHora = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    ImagenPrincipal = table.Column<string>(type: "text", nullable: true),
                    ImagenesAdicionales = table.Column<string>(type: "text", nullable: true),
                    Calificacion = table.Column<decimal>(type: "numeric(3,2)", nullable: false, defaultValue: 0m),
                    NumeroCalificaciones = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Disponible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Amenidades = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_canchas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_canchas_users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_canchas_CreatedByUserId",
                table: "canchas",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_canchas_disponible",
                table: "canchas",
                column: "Disponible");

            migrationBuilder.CreateIndex(
                name: "IX_canchas_nombre",
                table: "canchas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_canchas_tipo_deporte",
                table: "canchas",
                column: "TipoDeporte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "canchas");
        }
    }
}
