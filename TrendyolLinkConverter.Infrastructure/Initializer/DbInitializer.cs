using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using TrendyolLinkConverter.Core.Models;

namespace TrendyolLinkConverter.Infrastructure.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceScopeFactory scopeFactory;

        public DbInitializer(IServiceScopeFactory _scopeFactory)
        {
            scopeFactory = _scopeFactory;
        }

        public void Initialize()
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    if (!context.Sections.Any())
                    {
                        context.Sections.Add(new Section() {Id=1,Name= "Kadın" } );
                        context.Sections.Add(new Section() { Id=2,Name= "Erkek" }) ;
                        context.Sections.Add(new Section() {Id=3,Name= "Süpermarket" } );
                        context.Sections.Add(new Section() {Id=4,Name="Çocuk" } );

                        context.SaveChanges();
                    }
                  

                }
            }
        }
    }
}