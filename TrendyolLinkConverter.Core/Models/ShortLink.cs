using System;
using System.Collections.Generic;
using System.Text;
using TrendyolLinkConverter.Core.Exceptions;

namespace TrendyolLinkConverter.Core.Models
{
   public class ShortLink:BaseEntity
    {
        public string Code { get; set; }
        public string WebUrl { get; set; }
        public string DeepLink { get; set; }


        
    }
}
