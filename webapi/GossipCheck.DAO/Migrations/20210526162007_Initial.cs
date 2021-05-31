using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GossipCheck.DAO.Migrations
{
    public partial class Initial : Migration
    {
        private const string DefaultReputationsFile = @"DefaultData\defaultRep.csv";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceReputations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseUrl = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Reputation = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceReputations", x => x.Id);
                });

            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string theDirectory = Path.GetDirectoryName(fullPath);
            object[][] defaultData = File.ReadAllLines(Path.Combine(theDirectory, DefaultReputationsFile))[1..]
                .Select(x => new object[]
                {
                    x.Split(',')[0].Trim(),
                    DateTime.MinValue,
                    Convert.ToDouble(x.Split(',')[1].Trim())
                })
                .ToArray();

            foreach(var element in defaultData)
            {
                migrationBuilder.InsertData(
                    table: "SourceReputations",
                    columns: new[] { "BaseUrl", "Date", "Reputation" },
                    values: element);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceReputations");
        }
    }
}
