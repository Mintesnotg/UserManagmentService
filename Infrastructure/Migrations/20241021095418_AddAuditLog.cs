using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Discriminator",
            //    table: "UserRoleMappings");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Privileges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Privileges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Privileges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Privileges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RecordStatus",
                table: "Privileges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegisteredBy",
                table: "Privileges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredDate",
                table: "Privileges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Privileges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneInfo",
                table: "Privileges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Privileges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "RecordStatus",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "RegisteredBy",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "RegisteredDate",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "TimeZoneInfo",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Privileges");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "UserRoleMappings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
