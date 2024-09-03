using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Events.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Address", "Category", "DateTime", "Description", "ImagePath", "MaxPeopleCount", "Name" },
                values: new object[,]
                {
                    { 1, "Minsk 123", 0, new DateTime(2024, 9, 4, 0, 55, 41, 732, DateTimeKind.Utc).AddTicks(6102), "Top level concert", "concert.png", 4, "Concert" },
                    { 2, "Mos cow, 12", 1, new DateTime(2024, 9, 4, 0, 55, 41, 732, DateTimeKind.Utc).AddTicks(6126), "description ...", "meeting.png", 10, "Allowed meeting" },
                    { 3, "Paris, Sena", 2, new DateTime(2024, 9, 4, 0, 55, 41, 732, DateTimeKind.Utc).AddTicks(6129), "Frogs?", "paris.jpg", 9, "Fair with tail" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Email", "Name", "Password", "Surname" },
                values: new object[,]
                {
                    { 1, new DateOnly(1, 1, 1), "lol@gmail.com", "Pasha", "Pass123", "First" },
                    { 2, new DateOnly(1, 1, 1), "crol@mail.ru", "Petia", "Vass123", "Second" },
                    { 3, new DateOnly(1, 1, 1), "esc@gmama.help", "Vova", "Kiss123", "Third" }
                });

            migrationBuilder.InsertData(
                table: "Participation",
                columns: new[] { "Id", "EventId", "RegistrationTime", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Participation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Participation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Participation",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
