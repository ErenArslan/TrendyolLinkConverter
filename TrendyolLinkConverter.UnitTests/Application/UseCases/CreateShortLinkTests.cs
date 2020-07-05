using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;
using TrendyolLinkConverter.Core.UseCases;
using Xunit;

namespace TrendyolLinkConverter.UnitTests.Application.UseCases
{
   public class CreateShortLinkTests
    {
        [Fact]
        public async void CreateDeepLinkFromUrlHandler_InValidUrl_Null()
        {

            var shortLinkRepo = new Mock<IRepository<ShortLink>>();
            var mediator = new Mock<IMediator>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateShortlinkHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);



            var request = new CreateShortLinkRequest() { WebURL = "https://www.trendyol.com/casio/erkek-kol-saati-p-?boutiqueId=439892&merchantId=105064" };
            var sut = new CreateShortlinkHandler(shortLinkRepo.Object, mediator.Object, logger.Object, webUrlParser, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);


        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_InValidDeepLink_Null()
        {

            var shortLinkRepo = new Mock<IRepository<ShortLink>>();
            var mediator = new Mock<IMediator>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateShortlinkHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);



            var request = new CreateShortLinkRequest() { DeepLink = "ty://?Page=Product" };
            var sut = new CreateShortlinkHandler(shortLinkRepo.Object, mediator.Object, logger.Object, webUrlParser, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);


        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_ValidDeepLink_Success()
        {

            var shortLinkRepo = new Mock<IRepository<ShortLink>>();
            var mediator = new Mock<IMediator>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateShortlinkHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);



            var request = new CreateShortLinkRequest() { WebURL = "ty://?Page=Product&ContentId=1925865" };
            var sut = new CreateShortlinkHandler(shortLinkRepo.Object, mediator.Object, logger.Object, webUrlParser, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);


        }
    }
}
