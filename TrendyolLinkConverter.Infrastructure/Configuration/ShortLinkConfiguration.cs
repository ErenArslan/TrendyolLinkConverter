using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrendyolLinkConverter.Core.Models;

namespace TrendyolLinkConverter.Infrastructure.Configuration
{
   public class ShortLinkConfiguration : IEntityTypeConfiguration<ShortLink>
    {
        public void Configure(EntityTypeBuilder<ShortLink> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd()
                    .IsRequired();
            builder.Property(t => t.WebUrl)
                    .IsRequired();
            builder.Property(t => t.DeepLink)
                    .IsRequired();
            builder.Property(t => t.Code)
                   .IsRequired();

            builder.Property(t => t.CreateAt)
               .IsRequired();





        }
    }
}
