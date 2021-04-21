using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolAttendance.Data.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "ClassName", "Grade" },
                values: new object[,]
                {
                    { 1, "Class 1", "7" },
                    { 2, "Class 2", "8" },
                    { 3, "Class 3", "9" },
                    { 4, "Class 4", "10" },
                    { 5, "Class 5", "11" },
                    { 6, "Class 6", "12" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "FirstName", "LastName", "IDNumber" },
                values: new object[,]
                {
                    { 1, "Tom", "Wilson", "8010101234123" },
                    { 2, "Lisa", "Wilson", "8010101234123" },
                });

            migrationBuilder.InsertData(
                table: "StudentRegistrations",
                columns: new[] { "Id", "SchoolClassId", "StudentId", "HourOfDay" },
                values: new object[,]
                {
                    { 1, 1, 1, "8 am" },
                    { 2, 1, 2, "8 am" },
                });

            /*migrationBuilder.InsertData(
                table: "Attendance",
                columns: new[] { "Id", "StudentRegistrationId", "ClassDate", "Attended" },
                values: new object[,]
                {
                    { 1, 1, new System.DateTime(2021, 04, 21), 0 },
                    { 2, 2, new System.DateTime(2021, 04, 21), 1 },
                });*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
