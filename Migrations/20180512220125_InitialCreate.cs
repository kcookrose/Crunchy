using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Crunchy.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectItems",
                columns: table => new
                {
                    Pid = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectItems", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "StatusItems",
                columns: table => new
                {
                    Sid = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusItems", x => x.Sid);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    Uid = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    ProjectItemPid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_UserItems_ProjectItems_ProjectItemPid",
                        column: x => x.ProjectItemPid,
                        principalTable: "ProjectItems",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Tid = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssigneeUid = table.Column<long>(nullable: true),
                    DueDateTime = table.Column<DateTime>(nullable: false),
                    EstimatedTime = table.Column<TimeSpan>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OwnerProjectPid = table.Column<long>(nullable: true),
                    OwnerTodoItemTid = table.Column<long>(nullable: true),
                    StartDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Tid);
                    table.ForeignKey(
                        name: "FK_TodoItems_UserItems_AssigneeUid",
                        column: x => x.AssigneeUid,
                        principalTable: "UserItems",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoItems_ProjectItems_OwnerProjectPid",
                        column: x => x.OwnerProjectPid,
                        principalTable: "ProjectItems",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoItems_TodoItems_OwnerTodoItemTid",
                        column: x => x.OwnerTodoItemTid,
                        principalTable: "TodoItems",
                        principalColumn: "Tid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileRef",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RepoUrl = table.Column<string>(nullable: true),
                    TodoItemTid = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileRef", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileRef_TodoItems_TodoItemTid",
                        column: x => x.TodoItemTid,
                        principalTable: "TodoItems",
                        principalColumn: "Tid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileRef_TodoItemTid",
                table: "FileRef",
                column: "TodoItemTid");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_AssigneeUid",
                table: "TodoItems",
                column: "AssigneeUid");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_OwnerProjectPid",
                table: "TodoItems",
                column: "OwnerProjectPid");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_OwnerTodoItemTid",
                table: "TodoItems",
                column: "OwnerTodoItemTid");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ProjectItemPid",
                table: "UserItems",
                column: "ProjectItemPid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileRef");

            migrationBuilder.DropTable(
                name: "StatusItems");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "ProjectItems");
        }
    }
}
