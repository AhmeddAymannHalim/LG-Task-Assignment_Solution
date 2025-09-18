using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGTask.Assignment.DAL.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddnewCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "CategoryProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "CategoryProperties",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties",
                column: "PropertyId",
                principalTable: "PropertyItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "CategoryProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "CategoryProperties",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties",
                column: "PropertyId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
