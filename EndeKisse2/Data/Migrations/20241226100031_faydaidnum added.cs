using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EndeKisse2.Data.Migrations
{
    /// <inheritdoc />
    public partial class faydaidnumadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaydaIdNum",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaydaIdNum",
                table: "AspNetUsers");
        }
    }
}
