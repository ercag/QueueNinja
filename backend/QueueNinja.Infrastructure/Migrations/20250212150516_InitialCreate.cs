using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QueueNinja.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoredInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConnectionString = table.Column<string>(type: "text", nullable: false),
                    CreateOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitoredInstance_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserTenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTenants_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTenants_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredInstance_TenantId",
                table: "MonitoredInstance",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenants_TenantId",
                table: "UserTenants",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTenants_UserId",
                table: "UserTenants",
                column: "UserId");

            HangfireUp(migrationBuilder);

            // Insert dummy monitored instances
            migrationBuilder.Sql(@"
                INSERT INTO ""MonitoredInstance"" (""Name"", ""ConnectionString"", ""CreateOnUtc"")
                SELECT 'Test Instance', 'Host=localhost;Database=queueninja;Username=postgres;Password=postgres', NOW()
                WHERE NOT EXISTS (SELECT 1 FROM ""MonitoredInstance"" WHERE ""Name"" = 'Test Instance');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredInstance");

            migrationBuilder.DropTable(
                name: "UserTenants");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Tenants");

            HangfireDown(migrationBuilder);
        }

        private void HangfireUp(MigrationBuilder migrationBuilder)
        {
            // 🔹 Ensure the Hangfire schema exists
            migrationBuilder.Sql(@"CREATE SCHEMA IF NOT EXISTS hangfire;");

            // 🔹 Create Hangfire tables manually using Hangfire's naming convention
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS hangfire.job (
                    id SERIAL PRIMARY KEY,
                    invocationdata TEXT NOT NULL,
                    arguments TEXT NOT NULL,
                    createdat TIMESTAMP NOT NULL,
                    expireat TIMESTAMP NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.state (
                    id SERIAL PRIMARY KEY,
                    jobid INT NOT NULL REFERENCES hangfire.job(id),
                    statename TEXT NOT NULL,
                    reason TEXT NULL,
                    createdat TIMESTAMP NOT NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.jobqueue (
                    jobid INT NOT NULL REFERENCES hangfire.job(id),
                    queue TEXT NOT NULL,
                    fetchedat TIMESTAMP NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.server (
                    id TEXT PRIMARY KEY,
                    data TEXT NOT NULL,
                    lastheartbeat TIMESTAMP NOT NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.counter (
                    key TEXT PRIMARY KEY,
                    value INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.set (
                    key TEXT NOT NULL,
                    value TEXT NOT NULL,
                    score REAL NOT NULL,
                    expireat TIMESTAMP NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.hash (
                    key TEXT NOT NULL,
                    field TEXT NOT NULL,
                    value TEXT NOT NULL,
                    expireat TIMESTAMP NULL
                );

                CREATE TABLE IF NOT EXISTS hangfire.list (
                    key TEXT NOT NULL,
                    value TEXT NOT NULL,
                    expireat TIMESTAMP NULL
                );
                  
                CREATE TABLE IF NOT EXISTS hangfire.jobparameter (
                    jobid INT NOT NULL REFERENCES hangfire.job(id),
                    name TEXT NOT NULL,
                    value TEXT NOT NULL
                );
            ");

            SeedHangfireTestData(migrationBuilder);
        }

        private void HangfireDown(MigrationBuilder migrationBuilder)
        {
            // Drop all Hangfire tables if rolling back the migration
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.list;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.hash;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.set;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.counter;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.server;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.jobqueue;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.state;");
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS hangfire.job;");
            migrationBuilder.Sql(@"DROP SCHEMA IF EXISTS hangfire;");
        }
        // ✅ Function to Seed Dummy Test Data into Hangfire
        private void SeedHangfireTestData(MigrationBuilder migrationBuilder)
        {
            // Insert test jobs covering different states
            migrationBuilder.Sql(@"
        INSERT INTO hangfire.job (invocationdata, arguments, createdat, expireat)
        VALUES
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunSuccess"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW(), NOW() + INTERVAL '7 days'), -- Job 1 (Success)
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunFailure"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW(), NOW() + INTERVAL '7 days'), -- Job 2 (Failure)
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunLongTask"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW(), NOW() + INTERVAL '7 days'), -- Job 3 (Long-running)
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunDeleted"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW(), NOW() + INTERVAL '7 days'), -- Job 4 (Deleted)
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunScheduled"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW() + INTERVAL '1 hour', NOW() + INTERVAL '8 days'), -- Job 5 (Scheduled)
          ('{""Type"":""ExampleJob, MyApp"",""Method"":""RunRecurring"",""ParameterTypes"":[],""Arguments"":""[]""}', '[]', NOW(), NULL) -- Job 6 (Recurring)
        RETURNING id;
    ");

            // Insert job states (Processing, Succeeded, Failed, Deleted, Scheduled)
            migrationBuilder.Sql(@"
        INSERT INTO hangfire.state (jobid, statename, reason, createdat)
        VALUES
          (1, 'processing', NULL, NOW() - INTERVAL '10 minutes'),
          (1, 'succeeded', NULL, NOW()),

          (2, 'processing', NULL, NOW() - INTERVAL '5 minutes'),
          (2, 'failed', 'Unhandled exception', NOW()),

          (3, 'processing', NULL, NOW() - INTERVAL '20 minutes'),
          (3, 'succeeded', NULL, NOW()),

          (4, 'processing', NULL, NOW() - INTERVAL '15 minutes'),
          (4, 'deleted', 'Job was manually removed', NOW()),

          (5, 'scheduled', NULL, NOW() + INTERVAL '1 hour'),

          (6, 'recurring', NULL, NOW());
    ");

            // Insert job queue with multiple priority levels
            migrationBuilder.Sql(@"
        INSERT INTO hangfire.jobqueue (jobid, queue, fetchedat)
        VALUES
          (2, 'default', NULL), -- Failed job (Ready for retry)
          (3, 'critical', NULL), -- Long task (High priority)
          (5, 'low', NULL); -- Scheduled job (Lower priority)
    ");

            // Insert recurring job definitions
            migrationBuilder.Sql(@"
        INSERT INTO hangfire.hash (key, field, value)
        VALUES
          ('recurring-job:runRecurring', 'cron', '*/5 * * * *'),
          ('recurring-job:runRecurring', 'queue', 'default'),
          ('recurring-job:runRecurring', 'createdat', NOW()::TEXT);
    ");

            migrationBuilder.Sql(@"
        INSERT INTO hangfire.jobparameter (jobid, name, value)
        VALUES
          (1, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.'),
          (2, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.'),
          (3, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.'),
          (4, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.'),
          (5, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.'),
          (6, 'ExceptionDetails', 'System.NullReferenceException: Object reference not set to an instance of an object.');
    ");
        }
    }
}
