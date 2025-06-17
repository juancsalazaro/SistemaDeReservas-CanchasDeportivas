using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SistemaReservasApi.Migrations
{
    /// <inheritdoc />
    public partial class AddReservasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanchaId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "date", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Pendiente"),
                    NombreCliente = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailCliente = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TelefonoCliente = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MetodoPago = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reservas_canchas_CanchaId",
                        column: x => x.CanchaId,
                        principalTable: "canchas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservas_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservas_cancha_fecha",
                table: "reservas",
                columns: new[] { "CanchaId", "FechaReserva" });

            migrationBuilder.CreateIndex(
                name: "IX_reservas_estado",
                table: "reservas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_fecha",
                table: "reservas",
                column: "FechaReserva");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UserId",
                table: "reservas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas");
        }
    }
}
