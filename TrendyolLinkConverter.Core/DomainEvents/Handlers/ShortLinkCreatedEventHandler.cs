using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.DomainEvents.Events;

namespace TrendyolLinkConverter.Core.DomainEvents.Handlers
{
   public class ShortLinkCreatedEventHandler : INotificationHandler<ShortLinkCreatedEvent>
    {
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<ShortLinkCreatedEventHandler> logger;

        public ShortLinkCreatedEventHandler(IDistributedCache _distributedCache, ILogger<ShortLinkCreatedEventHandler> _logger)
        {
            distributedCache = _distributedCache;
            logger = _logger;
        }

        public async Task Handle(ShortLinkCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
              await  distributedCache.SetStringAsync(notification.Code, JsonConvert.SerializeObject(notification));

            }
            catch (Exception ex)
            {

                logger.LogError(ex.Message, "Time: " + DateTime.Now);

            }
        }
    }
}
