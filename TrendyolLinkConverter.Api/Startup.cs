using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrendyolLinkConverter.Api.Filters;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;
using TrendyolLinkConverter.Core.Validations;
using TrendyolLinkConverter.Core.UseCases;
using TrendyolLinkConverter.Infrastructure;
using TrendyolLinkConverter.Infrastructure.Initializer;
using TrendyolLinkConverter.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TrendyolLinkConverter.Api
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
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidateModelStateFilter());

            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });


            var hcBuilder = services.AddHealthChecks();

            hcBuilder
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddMySql(
                    Configuration.GetConnectionString("DefaultConnection"),
                    name: "MysqlDb-check",
                    tags: new string[] { "MysqlDb" });

           


            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IWebUrlParserService, WebUrlParserService>();

            services.AddMediatR(typeof(CreateDeepLinkFromUrlHandler));

            services.AddTransient<IValidator<CreateDeepLinkFromUrlRequest>, CreateDeepLinkFromUrlValidation>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            InitializeDb(app);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
        }

        public void InitializeDb(IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
                dbInitializer.Initialize();
                dbInitializer.SeedData();
            }
        }
    }
}
