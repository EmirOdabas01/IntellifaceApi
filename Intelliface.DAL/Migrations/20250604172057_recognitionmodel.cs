using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intelliface.DAL.Migrations
{
    /// <inheritdoc />
    public partial class recognitionmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FaceRecognitionModel",
                table: "Employees",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceRecognitionModel",
                table: "Employees");
        }
    }
}
