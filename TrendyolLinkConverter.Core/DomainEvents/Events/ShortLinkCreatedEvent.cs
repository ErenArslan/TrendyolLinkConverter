using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrendyolLinkConverter.Core.DomainEvents.Events
{
   public class ShortLinkCreatedEvent : INotification
    {
        public string Code { get; }
        public string WebUrl { get;  }
        public string DeepLink { get;  }


        public ShortLinkCreatedEvent(string code, string webUrl, string deepLink)
        {
            

            Code = code;
            WebUrl = webUrl;
            DeepLink = deepLink;
        }
    }
}
