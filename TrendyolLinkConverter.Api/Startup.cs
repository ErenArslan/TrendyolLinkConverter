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
using System;
using Microsoft.OpenApi.Models;

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
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"), p => p.EnableRetryOnFailure(maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddDistributedRedisCache(option => {
                option.Configuration = Configuration.GetSection("Redis").Value;
            });

            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IWebUrlParserService, WebUrlParserService>();
            services.AddTransient<IDeepLinkParserService, DeepLinkParserService>();

            services.AddMediatR(typeof(CreateDeepLinkFromUrlHandler));

            services.AddTransient<IValidator<CreateDeepLinkFromUrlRequest>, CreateDeepLinkFromUrlValidation>();
            services.AddTransient<IValidator<CreateUrlFromDeepLinkRequest>, CreateUrlFromDeepLinkValidation>();
            services.AddTransient<IValidator<GetLinksByShortLinkRequest>, GetLinksByShortLinkValidation>();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Trendyol Link Converter Api",
                    Version = "v1",
                    Description = "Link Converter HTTP API. This is a Data-Driven/CRUD microservice "
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDb(app);
            app.UseRouting();
            app.UseSwagger();

            app.UseSwaggerUI(p =>
            {
                p.SwaggerEndpoint("v1/swagger.json", "Swagger Test");
            });
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
