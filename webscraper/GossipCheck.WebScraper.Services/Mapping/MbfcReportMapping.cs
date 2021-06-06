using GossipCheck.WebScraper.Services.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GossipCheck.WebScraper.Services.Mapping
{
    public static class MbfcReportMapping
    {
        public static MbfcReport ToMbfcReport(this Dictionary<string, string> keyValues)
        {
            var report = new MbfcReport();

            report.Source = keyValues.GetValueOrDefault(nameof(MbfcReport.Source));
            report.PageUrl = keyValues.GetValueOrDefault(nameof(MbfcReport.PageUrl));

            report.Reasoning =
                keyValues.GetValueOrDefault(nameof(MbfcReport.Reasoning))
                ?? keyValues.GetValueOrDefault("QuestionableReasoning");

            var countryString =
                keyValues.GetValueOrDefault(nameof(MbfcReport.Country))
                ?? keyValues.GetValueOrDefault("County");

            var pressFreedomRankString =
                keyValues.GetValueOrDefault(nameof(MbfcReport.WorldPressFreedomRank))
                ?? Regex.Match(countryString ?? string.Empty, @"\d+\/\d+(?=\sPress\sFreedom)")?.Value;

            report.Country = Regex.Match(countryString ?? string.Empty, @".+?(?=\s\(|$)")?.Value;

            if (int.TryParse(pressFreedomRankString.Split('/')?[0], out int pressFreedomRank))
            {
                report.WorldPressFreedomRank = pressFreedomRank;
            }

            if (Enum.TryParse(keyValues.GetValueOrDefault(nameof(MbfcReport.FactualReporting))?.ToPascalCase(), out FactualReporting factualReporting))
            {
                report.FactualReporting = factualReporting;
            }

            if (Enum.TryParse(keyValues.GetValueOrDefault(nameof(MbfcReport.BiasRating))?.ToPascalCase(), out BiasRating biasRating))
            {
                report.BiasRating = biasRating;
            }
            else if (Enum.TryParse(keyValues.GetValueOrDefault("Bias"), out biasRating))
            {
                report.BiasRating = biasRating;
            }

            if (Enum.TryParse(keyValues.GetValueOrDefault(nameof(MbfcReport.MediaType))?.ToPascalCase(), out MediaType mediaType))
            {
                report.MediaType = mediaType;
            }

            if (Enum.TryParse(keyValues.GetValueOrDefault(nameof(MbfcReport.TrafficPopularity))?.ToPascalCase(), out TrafficPopularity trafficPopularity))
            {
                report.TrafficPopularity = trafficPopularity;
            }

            if (Enum.TryParse(keyValues.GetValueOrDefault(nameof(MbfcReport.MbfcCredibilityRating))?.ToPascalCase(), out MbfcCredibilityRating mbfcCredibility))
            {
                report.MbfcCredibilityRating = mbfcCredibility;
            }

            return report;
        }
    }
}
