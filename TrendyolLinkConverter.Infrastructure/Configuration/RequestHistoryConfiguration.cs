using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using TrendyolLinkConverter.Core.Models;

namespace TrendyolLinkConverter.Infrastructure.Configuration
{
   public class RequestHistoryConfiguration : IEntityTypeConfiguration<RequestHistory>
    {
        public void Configure(EntityTypeBuilder<RequestHistory> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd()
                    .IsRequired();
            builder.Property(t => t.Request)
                    .IsRequired();
            builder.Property(t => t.Response)
                    .IsRequired();


            builder.Property(t => t.CreateAt)
               .IsRequired();
             




        }
    }
}
