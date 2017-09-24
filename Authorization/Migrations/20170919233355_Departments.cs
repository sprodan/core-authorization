using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Authorization.Migrations
{
    public partial class Departments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Departments_DepartmentId",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_DepartmentId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Positions");

            migrationBuilder.AddColumn<int>(
                name: "IdDepartment",
                table: "Positions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_IdDepartment",
                table: "Positions",
                column: "IdDepartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Departments_IdDepartment",
                table: "Positions",
                column: "IdDepartment",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Departments_IdDepartment",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_IdDepartment",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "IdDepartment",
                table: "Positions");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Positions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_DepartmentId",
                table: "Positions",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Departments_DepartmentId",
                table: "Positions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
