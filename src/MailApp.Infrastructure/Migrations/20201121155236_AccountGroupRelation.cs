using Microsoft.EntityFrameworkCore.Migrations;

namespace MailApp.Infrastructure.Migrations
{
    public partial class AccountGroupRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Group_GroupId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_GroupId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Account");

            migrationBuilder.CreateTable(
                name: "GroupAccount",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccount", x => new { x.AccountId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_GroupAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAccount_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_AccountId",
                table: "GroupAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_GroupId",
                table: "GroupAccount",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupAccount");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_GroupId",
                table: "Account",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Group_GroupId",
                table: "Account",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
