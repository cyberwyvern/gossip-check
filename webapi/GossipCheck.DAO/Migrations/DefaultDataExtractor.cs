using GossipCheck.DAO.Entities;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GossipCheck.DAO.Migrations
{
    internal static class DefaultDataExtractor
    {
        public static IEnumerable<MbfcReport> ExtractData()
        {
            string rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = Path.Combine(rootDir, @"DefaultData\defaultData.csv");

            DateTime dateAdded = DateTime.Now;
            using TextFieldParser parser = new TextFieldParser(fullPath)
            {
                TextFieldType = FieldType.Delimited
            };

            List<MbfcReport> reports = new List<MbfcReport>();
            parser.SetDelimiters(",");
            parser.ReadFields();
            int id = 1;
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                reports.Add(new MbfcReport
                {
                    Id = id++,
                    Source = fields[0],
                    PageUrl = fields[9],
                    FactualReporting = Enum.TryParse(fields[1], out FactualReporting factuality) ? factuality : FactualReporting.NA,
                    BiasRating = Enum.TryParse(fields[2], out BiasRating bias) ? bias : default,
                    MediaType = Enum.TryParse(fields[3], out MediaType mediaType) ? mediaType : default,
                    TrafficPopularity = Enum.TryParse(fields[4], out TrafficPopularity traffic) ? traffic : default,
                    MbfcCredibilityRating = Enum.TryParse(fields[5], out MbfcCredibilityRating rating) ? rating : default,
                    Reasoning = fields[6],
                    Country = fields[7],
                    WorldPressFreedomRank = int.TryParse(fields[8], out int rank) ? rank : default,
                    Date = dateAdded
                });
            }

            return reports;
        }
    }
}