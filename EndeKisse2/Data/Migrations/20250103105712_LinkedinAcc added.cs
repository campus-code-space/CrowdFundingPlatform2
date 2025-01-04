using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EndeKisse2.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkedinAccadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedinAcc",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedinAcc",
                table: "AspNetUsers");
        }
    }
}
