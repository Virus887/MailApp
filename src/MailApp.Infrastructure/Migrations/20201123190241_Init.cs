using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MailApp.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    Nick = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "GroupAccountType",
                columns: table => new
                {
                    GroupAccountTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccountType", x => x.GroupAccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Notification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                });

            migrationBuilder.CreateTable(
                name: "MessagePersonType",
                columns: table => new
                {
                    MessagePersonTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagePersonType", x => x.MessagePersonTypeId);
                });

            migrationBuilder.CreateTable(
                name: "GroupAccount",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccount", x => new { x.AccountId, x.GroupId, x.TypeId });
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
                    table.ForeignKey(
                        name: "FK_GroupAccount_GroupAccountType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "GroupAccountType",
                        principalColumn: "GroupAccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessagePerson",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagePerson", x => new { x.AccountId, x.MessageId, x.TypeId });
                    table.ForeignKey(
                        name: "FK_MessagePerson_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessagePerson_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessagePerson_MessagePersonType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MessagePersonType",
                        principalColumn: "MessagePersonTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GroupAccountType",
                columns: new[] { "GroupAccountTypeId", "Name" },
                values: new object[,]
                {
                    { 2, "Member" },
                    { 1, "Owner" }
                });

            migrationBuilder.InsertData(
                table: "MessagePersonType",
                columns: new[] { "MessagePersonTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Sender" },
                    { 2, "Receiver" },
                    { 3, "Cc" },
                    { 4, "Bcc" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_Nick",
                table: "Account",
                column: "Nick",
                unique: true,
                filter: "[Nick] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Group_Name",
                table: "Group",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_AccountId",
                table: "GroupAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_GroupId",
                table: "GroupAccount",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccount_TypeId",
                table: "GroupAccount",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagePerson_AccountId",
                table: "MessagePerson",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagePerson_MessageId",
                table: "MessagePerson",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagePerson_TypeId",
                table: "MessagePerson",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupAccount");

            migrationBuilder.DropTable(
                name: "MessagePerson");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "GroupAccountType");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MessagePersonType");
        }
    }
}
