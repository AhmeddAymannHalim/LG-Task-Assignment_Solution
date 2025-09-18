using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LGTask.Assignment.DAL.Persistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedDateIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "AcquisitionDate",
                table: "Devices",
                type: "date",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AcquisitionDate",
                table: "Devices",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
