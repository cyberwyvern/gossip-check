using GossipCheck.WebScraper.ConfigurationOptionModels;
using GossipCheck.WebScraper.Services;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var scraperConfig = this.Configuration.GetSection(ScraperConfigurationSection);
            var nluServiceConfig = this.Configuration.GetSection(NLUServiceConfigurationSection);

            services.AddOptions<ScraperServiceConfig>().Bind(scraperConfig);
            services.AddOptions<NLUServiceConfig>().Bind(nluServiceConfig);

            services.AddTransient<INLUService, NLUService>();
            services.AddTransient<IWebScraperService, WebScraperService>();

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

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
