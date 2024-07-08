using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Context.Migrations
{
    /// <inheritdoc />
    public partial class AlterMainSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemPerPage",
                table: "MainSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemPerPage",
                table: "MainSettings");
        }
    }
}
