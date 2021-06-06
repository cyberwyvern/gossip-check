using System;
using System.Linq;
using GossipCheck.DAO.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GossipCheck.DAO.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MbfcReports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(nullable: true),
                    PageUrl = table.Column<string>(nullable: true),
                    FactualReporting = table.Column<int>(nullable: false),
                    BiasRating = table.Column<int>(nullable: false),
                    MediaType = table.Column<int>(nullable: false),
                    TrafficPopularity = table.Column<int>(nullable: false),
                    MbfcCredibilityRating = table.Column<int>(nullable: false),
                    Reasoning = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    WorldPressFreedomRank = table.Column<int>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbfcReports", x => x.Id);
                });

            var defaultData = DefaultDataExtractor.ExtractData();
            var properties = typeof(MbfcReport).GetProperties();
            foreach (var report in defaultData)
            {
                migrationBuilder.InsertData(
                    "MbfcReports",
                    properties.Select(x => x.Name).ToArray(),
                    properties.Select(x => x.GetValue(report).GetType().IsEnum ? (int)x.GetValue(report) : x.GetValue(report))
                    .ToArray());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MbfcReports");
        }
    }
}
