using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class addLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ExpenseLimit",
                table: "AspNetUsers",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseLimit",
                table: "AspNetUsers");
        }
    }
}
