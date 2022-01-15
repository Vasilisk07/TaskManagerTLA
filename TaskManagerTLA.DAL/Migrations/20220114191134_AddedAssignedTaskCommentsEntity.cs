using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskManagerTLA.DAL.Migrations
{
    public partial class AddedAssignedTaskCommentsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a6b6442-20bc-49b1-babf-412bf422dd97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af413399-eb29-4d54-90c9-1c884b7e731d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0a982db-0c64-4a09-9e0f-7a3dee9dc27a");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssignedTask");

            migrationBuilder.CreateTable(
                name: "AssignedTaskComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedTaskGlobalTaskId = table.Column<int>(type: "int", nullable: true),
                    AssignedTaskUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedTaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedTaskComments_AssignedTask_AssignedTaskGlobalTaskId_AssignedTaskUserId",
                        columns: x => new { x.AssignedTaskGlobalTaskId, x.AssignedTaskUserId },
                        principalTable: "AssignedTask",
                        principalColumns: new[] { "GlobalTaskId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bb0b0904-0b4a-4593-b908-08996061ccd5", "04896047-da09-4c39-bbdc-92250959f5ce", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36783d12-c304-4b85-b334-6dfcede73bde", "9e23bfe1-5a59-4871-a926-33b0e0722177", "Manager", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "63360200-50fa-4636-9562-5396f4c8912a", "39030ab3-1e2c-4911-8cc7-b55dcc219c7b", "Developer", null });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedTaskComments_AssignedTaskGlobalTaskId_AssignedTaskUserId",
                table: "AssignedTaskComments",
                columns: new[] { "AssignedTaskGlobalTaskId", "AssignedTaskUserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedTaskComments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36783d12-c304-4b85-b334-6dfcede73bde");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "63360200-50fa-4636-9562-5396f4c8912a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb0b0904-0b4a-4593-b908-08996061ccd5");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssignedTask",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "af413399-eb29-4d54-90c9-1c884b7e731d", "31877a4a-802a-477b-a55c-8d9725d6f8df", "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6a6b6442-20bc-49b1-babf-412bf422dd97", "454b0bab-9fdb-4b0e-89a0-2f63890ad540", "Manager", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d0a982db-0c64-4a09-9e0f-7a3dee9dc27a", "5eeb4aaf-0726-4c9a-8735-d3f41a26ef76", "Developer", null });
        }
    }
}
