using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;
using TrendyolLinkConverter.Core.UseCases;
using Xunit;
using System.Linq.Expressions;

namespace TrendyolLinkConverter.UnitTests.Application.UseCases
{
   public class CreateUrlFromDeepLinkTests
    {
        [Fact]
        public async void CreateUrlFromDeepLinkHandler_ProductPageNoContentId_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Product&CampaignId=439892&MerchantId=105064" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);


        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_ProductPageWithOutQueryString_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Product&ContentId=1925865" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);

            var resultOutput = result.Data.WebUrl;
            var expectedOutput = "https://www.trendyol.com/brand/name-p-1925865";

            Assert.Equal(resultOutput, expectedOutput);
        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_ProductPageWithQueryString_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Product&ContentId=1925865&CampaignId=439892&MerchantId=105064" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);

            var resultOutput = result.Data.WebUrl;
            var expectedOutput = "https://www.trendyol.com/brand/name-p-1925865?boutiqueId=439892&merchantId=105064";

            Assert.Equal(resultOutput, expectedOutput);
        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_SearchPageNoQuery_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Search" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);

        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_SearchPageValidQuery_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Search&Query=elbise" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);
            var resultOutput = result.Data.WebUrl;
            var expectedOutput = "https://www.trendyol.com/tum--urunler?q=elbise";

            Assert.Equal(resultOutput, expectedOutput);

        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_OtherPageValid_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Favorites" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);
            var resultOutput = result.Data.WebUrl;
            var expectedOutput = "https://www.trendyol.com";

            Assert.Equal(resultOutput, expectedOutput);

        }

        [Fact]
        public async void CreateUrlFromDeepLinkHandler_HomePagedNoSctionId_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            sctionRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Section, bool>>>())).Returns(LoadSection_Erkek());

            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Home" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);
        }

      



        [Fact]
        public async void CreateUrlFromDeepLinkHandler_HomePageValid_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            sctionRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Section, bool>>>())).Returns(LoadSection_Erkek());

            var logger = new Mock<ILogger<CreateUrlFromDeepLinkHandler>>();
            IDeepLinkParserService deepLinkParser = new DeepLinkParserService(sctionRepo.Object);

            var request = new CreateUrlFromDeepLinkRequest() { DeepLink = "ty://?Page=Home&SectionId=2" };
            var sut = new CreateUrlFromDeepLinkHandler(requestHistoryRepo.Object, logger.Object, deepLinkParser);
            var result = await sut.Handle(request, CancellationToken.None);

            Assert.NotNull(result.Data);

            var resultOutput = result.Data.WebUrl;
            var expectedOutput = "https://www.trendyol.com/butik/liste/erkek";

            Assert.Equal(resultOutput, expectedOutput);
        }


        async Task<Section> LoadSection_Erkek()
        {
            var t = Task.Run(() =>
            {
                return
                new Section() { Id = 1, Name = "Erkek" };
            });

            return await t;

        }
    }
}
