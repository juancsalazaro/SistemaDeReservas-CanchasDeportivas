using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaReservasApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRolToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Cliente");

            migrationBuilder.CreateIndex(
                name: "IX_users_rol",
                table: "users",
                column: "Rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_rol",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "users");
        }
    }
}
