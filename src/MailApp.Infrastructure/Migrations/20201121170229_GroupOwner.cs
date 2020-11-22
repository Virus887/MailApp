using Microsoft.EntityFrameworkCore.Migrations;

namespace MailApp.Infrastructure.Migrations
{
    public partial class GroupOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "GroupAccount",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GroupAccountType",
                columns: table => new
                {
                    GroupAccountTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccountType", x => x.GroupAccountTypeId);
                });

            migrationBuilder.InsertData(
                table: "GroupAccountType",
                column: "GroupAccountTypeId",
                value: 2);

            migrationBuilder.InsertData(
                table: "GroupAccountType",
                column: "GroupAccountTypeId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_TypeId",
                table: "GroupAccount",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAccount_GroupAccountType_TypeId",
                table: "GroupAccount",
                column: "TypeId",
                principalTable: "GroupAccountType",
                principalColumn: "GroupAccountTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAccount_GroupAccountType_TypeId",
                table: "GroupAccount");

            migrationBuilder.DropTable(
                name: "GroupAccountType");

            migrationBuilder.DropIndex(
                name: "IX_GroupAccount_TypeId",
                table: "GroupAccount");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "GroupAccount");
        }
    }
}
