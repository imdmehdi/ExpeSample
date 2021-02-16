using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseApproval.Data.Migrations
{
    public partial class userID_ExpenseStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpenseRequesterID",
                table: "Expense",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Expense",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseRequesterID",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Expense");
        }
    }
}
