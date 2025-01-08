using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EndeKisse2.Data.Migrations
{
    /// <inheritdoc />
    public partial class AmountaddedtoTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Transaction",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transaction");
        }
    }
}
