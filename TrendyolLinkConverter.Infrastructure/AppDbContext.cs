using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.IO;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Infrastructure.Configuration;

namespace TrendyolLinkConverter.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<RequestHistory> RequestHistories { get; set; }
        public DbSet<ShortLink> ShortLinks { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new SectionConfiguration());
            builder.ApplyConfiguration(new RequestHistoryConfiguration());
            builder.ApplyConfiguration(new ShortLinkConfiguration());

        }


    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables()
            //    .Build();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = "Server=mysql-db,3306;Database=LinkConverter;Uid=user;Pwd=admin;";
            builder.UseMySql(connectionString);
            return new AppDbContext(builder.Options);

        }
    }

}
