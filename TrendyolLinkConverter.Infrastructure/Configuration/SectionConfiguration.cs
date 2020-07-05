using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrendyolLinkConverter.Core.Models;

namespace TrendyolLinkConverter.Infrastructure.Configuration
{
    public  class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(250);
        

            builder.Property(t => t.CreateAt)
               .IsRequired();
          



        }
    }
}
