using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intelliface.DAL.Migrations
{
    /// <inheritdoc />
    public partial class trainimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTrainImage_Employees_EmployeeId",
                table: "EmployeeTrainImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTrainImage",
                table: "EmployeeTrainImage");

            migrationBuilder.RenameTable(
                name: "EmployeeTrainImage",
                newName: "EmployeeTrainImages");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeTrainImage_EmployeeId",
                table: "EmployeeTrainImages",
                newName: "IX_EmployeeTrainImages_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTrainImages",
                table: "EmployeeTrainImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTrainImages_Employees_EmployeeId",
                table: "EmployeeTrainImages",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeTrainImages_Employees_EmployeeId",
                table: "EmployeeTrainImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTrainImages",
                table: "EmployeeTrainImages");

            migrationBuilder.RenameTable(
                name: "EmployeeTrainImages",
                newName: "EmployeeTrainImage");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeTrainImages_EmployeeId",
                table: "EmployeeTrainImage",
                newName: "IX_EmployeeTrainImage_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTrainImage",
                table: "EmployeeTrainImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeTrainImage_Employees_EmployeeId",
                table: "EmployeeTrainImage",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
