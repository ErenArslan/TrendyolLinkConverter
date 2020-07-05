using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.UseCases;
using Xunit;

namespace TrendyolLinkConverter.UnitTests.Application.UseCases
{
  public  class GetLinksByShortLinkTests
    {
        [Fact]
        public async void CreateDeepLinkFromUrlHandler_EmptyShortLink_Null()
        {

            var shortLinkRepo = new Mock<IRepository<ShortLink>>();
            var cache = new Mock<IDistributedCache>();
            var logger = new Mock<ILogger<GetLinksByShortLinkHandler>>();
           
            var request = new GetLinksByShortLinkRequest() { ShortLink = "" };
            var sut = new GetLinksByShortLinkHandler(cache.Object, shortLinkRepo.Object, logger.Object);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);


        }

      
    }
}
