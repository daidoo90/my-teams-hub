using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTeamsHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "organizations");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Organization",
                schema: "organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    LogoBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHashBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordSaltBase64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoleTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserStatus = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                schema: "organizations",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "organizations",
                        principalTable: "Organization",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                schema: "organizations",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => new { x.TeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TeamMember_Team_TeamId",
                        column: x => x.TeamId,
                        principalSchema: "organizations",
                        principalTable: "Team",
                        principalColumn: "TeamId");
                    table.ForeignKey(
                        name: "FK_TeamMember_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Team_OrganizationId",
                schema: "organizations",
                table: "Team",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_UserId",
                schema: "organizations",
                table: "TeamMember",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMember",
                schema: "organizations");

            migrationBuilder.DropTable(
                name: "Team",
                schema: "organizations");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Organization",
                schema: "organizations");
        }
    }
}
