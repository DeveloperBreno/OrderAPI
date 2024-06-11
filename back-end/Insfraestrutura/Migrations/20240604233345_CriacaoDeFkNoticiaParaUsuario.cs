using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insfraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoDeFkNoticiaParaUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "TB_NOTICIA",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NOTICIA_UsuarioId",
                table: "TB_NOTICIA",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_NOTICIA_AspNetUsers_UsuarioId",
                table: "TB_NOTICIA",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_NOTICIA_AspNetUsers_UsuarioId",
                table: "TB_NOTICIA");

            migrationBuilder.DropIndex(
                name: "IX_TB_NOTICIA_UsuarioId",
                table: "TB_NOTICIA");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "TB_NOTICIA");
        }
    }
}
