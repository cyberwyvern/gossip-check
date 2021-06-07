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
        private const string ArticleSearchConfigurationSection = "ArticleSearch";
        private const string NLUServiceConfigurationSection = "NLUService";
        private const string MbfcServiceConfigurationSection = "MbfcService";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var articleSearchConfig = this.Configuration.GetSection(ArticleSearchConfigurationSection);
            var nluServiceConfig = this.Configuration.GetSection(NLUServiceConfigurationSection);
            var mbfcServiceConfig = this.Configuration.GetSection(MbfcServiceConfigurationSection);

            services.AddOptions<ArticleSearchEngineConfig>().Bind(articleSearchConfig);
            services.AddOptions<NLUServiceConfig>().Bind(nluServiceConfig);
            services.AddOptions<MbfcServiceConfig>().Bind(mbfcServiceConfig);

            services.AddTransient<INLUService, NLUService>();
            services.AddTransient<IArticleSearchEngine, ArticleSearchEngine>();
            services.AddTransient<IMbfcCrawler, MbfcCrawler>();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
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
