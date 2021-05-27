using GossipCheck.BLL;
using GossipCheck.BLL.Interface;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var stanceDetectorServiceConfig = Configuration.GetSection(StanceDetectorConfigurationSection);
            services.AddOptions<StanceDetectorServiceConfig>().Bind(stanceDetectorServiceConfig);

            services.AddDbContext<GossipCheckDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DBconnection"));
            });

            services.AddScoped<IGossipCheckUnitOfWork, GossipCheckUnitOfWork>();
            services.AddTransient<IReputabilityAlgorithm, ReputabilityAlgorithm>();
            services.AddTransient<IStanceDetectorFascade, StanceDetectorFascade>();

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
