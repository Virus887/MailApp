using Microsoft.EntityFrameworkCore.Migrations;

namespace MailApp.Infrastructure.Migrations
{
    public partial class MoveIsRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Message");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "MessagePerson",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "MessagePerson");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Message",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
