using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class initcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CyclingSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    When = table.Column<DateTime>(nullable: false),
                    Distance = table.Column<float>(nullable: true),
                    Time = table.Column<TimeSpan>(nullable: false),
                    AverageSpeed = table.Column<float>(nullable: true),
                    AverageWatt = table.Column<int>(nullable: true),
                    TrainingType = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    BikeType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CyclingSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RunningSessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    When = table.Column<DateTime>(nullable: false),
                    Distance = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    AverageSpeed = table.Column<float>(nullable: false),
                    TrainingType = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunningSessions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CyclingSessions");

            migrationBuilder.DropTable(
                name: "RunningSessions");
        }
    }
}
