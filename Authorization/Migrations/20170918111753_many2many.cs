using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Authorization.Migrations
{
    public partial class many2many : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Roles_RoleId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Modules_ModuleId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ModuleId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Modules_RoleId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Modules");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RoleModule",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdModule = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModule", x => new { x.IdRole, x.IdModule });
                    table.ForeignKey(
                        name: "FK_RoleModule_Modules_IdModule",
                        column: x => x.IdModule,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleModule_Roles_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleModule_IdModule",
                table: "RoleModule",
                column: "IdModule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleModule");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Roles",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "Roles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Modules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModuleId",
                table: "Roles",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_RoleId",
                table: "Modules",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Roles_RoleId",
                table: "Modules",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Modules_ModuleId",
                table: "Roles",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
