using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCepComplementoEndereco : Migration
    {
        /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cep",
                table: "Enderecos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "00000000"); // ⬅️ ALTERE ESTA LINHA AQUI

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Enderecos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cep",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Enderecos");
        }
    }
}
