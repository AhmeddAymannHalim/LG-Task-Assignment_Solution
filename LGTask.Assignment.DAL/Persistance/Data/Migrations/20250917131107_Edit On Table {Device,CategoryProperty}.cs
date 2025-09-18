using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGTask.Assignment.DAL.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOnTableDeviceCategoryProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "CategoryProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyItemId",
                table: "CategoryProperties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProperties_PropertyItemId",
                table: "CategoryProperties",
                column: "PropertyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyItemId",
                table: "CategoryProperties",
                column: "PropertyItemId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyItemId",
                table: "CategoryProperties");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProperties_PropertyItemId",
                table: "CategoryProperties");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "CategoryProperties");

            migrationBuilder.DropColumn(
                name: "PropertyItemId",
                table: "CategoryProperties");
        }
    }
}
