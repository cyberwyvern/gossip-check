using GossipCheck.WebScraper.Services.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace GossipCheck.WebScraper
{
    public class Startup
    {
        private const string ScraperConfigurationSection = "ScraperServiceConfig";
        private const string NLUServiceConfigurationSection = "NLUServiceConfig";
        private const string MbfcServiceConfigurationSection = "MbfcServiceConfig";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection scraperConfig = Configuration.GetSection(ScraperConfigurationSection);
            IConfigurationSection nluServiceConfig = Configuration.GetSection(NLUServiceConfigurationSection);
            IConfigurationSection mbfcServiceConfig = Configuration.GetSection(MbfcServiceConfigurationSection);

            services.AddOptions<ScraperServiceConfig>().Bind(scraperConfig);
            services.AddOptions<NLUServiceConfig>().Bind(nluServiceConfig);
            services.AddOptions<MbfcServiceConfig>().Bind(mbfcServiceConfig);

            services.AddTransient<INLUService, NLUService>();
            services.AddTransient<IWebScraperService, WebScraperService>();
            services.AddTransient<IMbfcCrawler, MbfcCrawler>();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
