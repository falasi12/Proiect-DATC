using APIAPP.Repositories;
using BackgroundWorker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace APIAPP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<InfoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPointOfInterestRepository, PointOfInterestRepository>();

            services.AddCors(options => {
                var allowedOrigin = Configuration.GetSection("AlloedOrigins").Get<string[]>();
                allowedOrigin.Concat(new List<string> { "Access-Control-Allow-Origin", "Access-Control-Allow-Credentials" });
                options.AddPolicy("MyPolicy", builder => builder
                .WithOrigins(allowedOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Content-Disposition")
                );
            
            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
       
    }
}
