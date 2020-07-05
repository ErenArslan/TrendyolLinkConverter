using System;
using System.Collections.Generic;
using System.Text;

namespace TrendyolLinkConverter.Core.Models
{
   public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateAt { get;private set; } = DateTime.Now;

              
    }
}
