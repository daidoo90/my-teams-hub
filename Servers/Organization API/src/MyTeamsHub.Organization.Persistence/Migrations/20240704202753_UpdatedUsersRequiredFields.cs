using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeamsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUsersRequiredFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSaltBase64",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHashBase64",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "organizations",
                table: "Team",
                keyColumn: "TeamId",
                keyValue: new Guid("a933bec8-abd6-40ce-893a-04d33b69ded5"));

            migrationBuilder.DeleteData(
                schema: "organizations",
                table: "Organization",
                keyColumn: "OrganizationId",
                keyValue: new Guid("9ab43667-428f-45ce-902b-a19e3d91cf5e"));

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                schema: "dbo",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordSaltBase64",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHashBase64",
                schema: "dbo",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                schema: "dbo",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.InsertData(
                schema: "organizations",
                table: "Organization",
                columns: new[] { "OrganizationId", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "LastUpdatedOn", "LogoBase64", "Name" },
                values: new object[] { new Guid("a77f79d3-008b-46ac-bbff-1781988de229"), new DateTime(2024, 6, 22, 21, 50, 7, 851, DateTimeKind.Utc).AddTicks(5786), null, "Company is delivering teams management services.", false, null, null, "MyTeamsHub Ltd" });

            migrationBuilder.InsertData(
                schema: "organizations",
                table: "Team",
                columns: new[] { "TeamId", "CreatedOn", "DeletedOn", "Description", "IsDeleted", "LastUpdatedOn", "Name", "OrganizationId" },
                values: new object[] { new Guid("7eafe81a-4c95-4dad-8d39-c6da6bab9070"), new DateTime(2024, 6, 22, 21, 50, 7, 852, DateTimeKind.Utc).AddTicks(3405), null, "Development team that is delivering mobile applications.", false, null, "Mobile Dev Team", new Guid("a77f79d3-008b-46ac-bbff-1781988de229") });
        }
    }
}
