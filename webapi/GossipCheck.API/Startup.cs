using GossipCheck.BLL.ConfigurationModels;
using GossipCheck.BLL.Interface;
using GossipCheck.BLL.Services;
using GossipCheck.DAO;
using GossipCheck.DAO.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace GossipCheck.API
{
    public class Startup
    {
        private const string StanceDetectorConfigurationSection = "StanceDetector";
        private const string MbfcServiceConfigurationSection = "MbfcService";

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var stanceDetectorServiceConfig = this.Configuration.GetSection(StanceDetectorConfigurationSection);
            var mbfcServiceConfig = this.Configuration.GetSection(MbfcServiceConfigurationSection);
            services.AddOptions<StanceDetectorServiceConfig>().Bind(stanceDetectorServiceConfig);
            services.AddOptions<MbfcServiceConfig>().Bind(mbfcServiceConfig);

            services.AddDbContext<GossipCheckDBContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("DBconnection"));
            });

            services.AddScoped<IGossipCheckUnitOfWork, GossipCheckUnitOfWork>();
            services.AddTransient<IMbfcFacade, MbfcFacade>();
            services.AddTransient<IReputabilityAlgorithm, ReputabilityAlgorithm>();
            services.AddTransient<IStanceDetectorFacade, StanceDetectorFacade>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
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
