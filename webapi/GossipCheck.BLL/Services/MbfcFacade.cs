using GossipCheck.BLL.ConfigurationModels;
using GossipCheck.BLL.Interface;
using GossipCheck.DAO.Entities;
using GossipCheck.DAO.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Services
{
    public class MbfcFacade : IMbfcFacade, IDisposable
    {
        private readonly HttpClient client;
        private readonly MbfcServiceConfig config;
        private readonly IGossipCheckUnitOfWork uow;

        public MbfcFacade(IOptions<MbfcServiceConfig> config, IGossipCheckUnitOfWork uow)
        {
            this.config = config.Value;
            this.uow = uow;
            this.client = new HttpClient
            {
                BaseAddress = new Uri(this.config.ServiceUrl)
            };
        }

        public async Task<IEnumerable<MbfcReport>> GetReportsAsync(IEnumerable<string> sourceUrls)
        {
            var reports = await GetReportsFromDatabase(sourceUrls);
            var missingReportUrls = sourceUrls.Except(reports.Select(x => x.Source));
            var missingReports = await GetReportsFromWeb(missingReportUrls);
            await SaveReports(missingReports);

            return reports.Concat(missingReports).ToList();
        }

        private async Task<IEnumerable<MbfcReport>> GetReportsFromWeb(IEnumerable<string> sourceUrls)
        {
            var tasks = sourceUrls.Select(x => GetReportFromWeb(x)).ToList();
            tasks.ForEach(x => x.Start());

            return (await Task.WhenAll(tasks)).Where(x => x != null).ToList();
        }

        private async Task<MbfcReport> GetReportFromWeb(string sourceUrl)
        {
            var response = await client.GetAsync(sourceUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var report = JsonConvert.DeserializeObject<MbfcReport>(content);

            return report;
        }

        private async Task<IEnumerable<MbfcReport>> GetReportsFromDatabase(IEnumerable<string> sourceUrls)
        {
            return await uow.MbfcReports.GetLatestByUrlsAsync(sourceUrls);
        }

        private async Task SaveReports(IEnumerable<MbfcReport> reports)
        {
            var reportsList = reports.ToList();
            var savedDate = DateTime.Now;
            reportsList.ForEach(x => x.Date = savedDate);

            await uow.MbfcReports.CreateMultipleAsync(reportsList);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
