using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTrackerApp.Migrations
{
    /// <inheritdoc />
    public partial class recurringJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RecurringTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RecurringTransactions");
        }
    }
}
