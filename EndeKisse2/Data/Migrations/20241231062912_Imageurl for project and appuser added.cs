using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EndeKisse2.Data.Migrations
{
    /// <inheritdoc />
    public partial class Imageurlforprojectandappuseradded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl1",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl2",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl3",
                table: "Project",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl1",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ImageUrl2",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ImageUrl3",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "IdImageUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserImageUrl",
                table: "AspNetUsers");
        }
    }
}
