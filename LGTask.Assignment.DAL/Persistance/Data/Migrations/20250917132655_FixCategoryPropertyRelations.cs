using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGTask.Assignment.DAL.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryPropertyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_DeviceCategories_DeviceCategoryId",
                table: "CategoryProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyItemId",
                table: "CategoryProperties");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProperties_DeviceCategoryId",
                table: "CategoryProperties");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProperties_PropertyItemId",
                table: "CategoryProperties");

            migrationBuilder.DropColumn(
                name: "DeviceCategoryId",
                table: "CategoryProperties");

            migrationBuilder.DropColumn(
                name: "PropertyItemId",
                table: "CategoryProperties");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProperties_CategoryId",
                table: "CategoryProperties",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProperties_PropertyId",
                table: "CategoryProperties",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_DeviceCategories_CategoryId",
                table: "CategoryProperties",
                column: "CategoryId",
                principalTable: "DeviceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties",
                column: "PropertyId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_DeviceCategories_CategoryId",
                table: "CategoryProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyId",
                table: "CategoryProperties");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProperties_CategoryId",
                table: "CategoryProperties");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProperties_PropertyId",
                table: "CategoryProperties");

            migrationBuilder.AddColumn<int>(
                name: "DeviceCategoryId",
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
                name: "IX_CategoryProperties_DeviceCategoryId",
                table: "CategoryProperties",
                column: "DeviceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProperties_PropertyItemId",
                table: "CategoryProperties",
                column: "PropertyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_DeviceCategories_DeviceCategoryId",
                table: "CategoryProperties",
                column: "DeviceCategoryId",
                principalTable: "DeviceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProperties_PropertyItems_PropertyItemId",
                table: "CategoryProperties",
                column: "PropertyItemId",
                principalTable: "PropertyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
