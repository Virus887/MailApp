using Microsoft.EntityFrameworkCore.Migrations;

namespace MailApp.Infrastructure.Migrations
{
    public partial class Account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Account",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Account_Email",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Account");
        }
    }
}
