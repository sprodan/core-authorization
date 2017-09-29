using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Authorization.Migrations
{
    public partial class many2many3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModule_Modules_IdModule",
                table: "RoleModule");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModule_Roles_IdRole",
                table: "RoleModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleModule",
                table: "RoleModule");

            migrationBuilder.RenameTable(
                name: "RoleModule",
                newName: "RoleModules");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModule_IdModule",
                table: "RoleModules",
                newName: "IX_RoleModules_IdModule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleModules",
                table: "RoleModules",
                columns: new[] { "IdRole", "IdModule" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Modules_IdModule",
                table: "RoleModules",
                column: "IdModule",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Roles_IdRole",
                table: "RoleModules",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Modules_IdModule",
                table: "RoleModules");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Roles_IdRole",
                table: "RoleModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleModules",
                table: "RoleModules");

            migrationBuilder.RenameTable(
                name: "RoleModules",
                newName: "RoleModule");

            migrationBuilder.RenameIndex(
                name: "IX_RoleModules_IdModule",
                table: "RoleModule",
                newName: "IX_RoleModule_IdModule");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleModule",
                table: "RoleModule",
                columns: new[] { "IdRole", "IdModule" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModule_Modules_IdModule",
                table: "RoleModule",
                column: "IdModule",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModule_Roles_IdRole",
                table: "RoleModule",
                column: "IdRole",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
