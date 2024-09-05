using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserClaimCorrectTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<int>",
                table: "IdentityUserClaim<int>");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<int>",
                newName: "UserClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 22, 50, 316, DateTimeKind.Utc).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 22, 50, 316, DateTimeKind.Utc).AddTicks(4951));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 22, 50, 316, DateTimeKind.Utc).AddTicks(4953));

            migrationBuilder.UpdateData(
                table: "UserClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "0f2b24cf-e4e7-4c17-ad7e-6dd7215371cb");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5de984a4-c7aa-4475-9e2d-2626c566b565");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "faf83382-280a-4edc-839e-f8362101dd65");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "IdentityUserClaim<int>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<int>",
                table: "IdentityUserClaim<int>",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 10, 14, 57, DateTimeKind.Utc).AddTicks(795));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 10, 14, 57, DateTimeKind.Utc).AddTicks(813));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2024, 9, 4, 23, 10, 14, 57, DateTimeKind.Utc).AddTicks(816));

            migrationBuilder.UpdateData(
                table: "IdentityUserClaim<int>",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "role");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "22e07d6f-72ef-4a44-aecb-723c21b51ddb");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5d7218e2-bad6-44f8-8dac-079f4af008ea");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "54af3307-6611-42f4-88db-f8c4c9572c46");
        }
    }
}
