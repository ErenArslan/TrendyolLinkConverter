using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;
using TrendyolLinkConverter.Core.UseCases;
using Xunit;

namespace TrendyolLinkConverter.UnitTests.Application.UseCases
{
    public class CreateDeepLinkFromUrlTests
    {



        [Fact]
        public async void CreateDeepLinkFromUrlHandler_ProductPageNoContentId_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/casio/erkek-kol-saati-p-?boutiqueId=439892&merchantId=105064" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);

          
        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_ProductPageWithOutQueryString_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/casio/erkek-kol-saati-p-1925865" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);

            var resultOutput = result.Data.DeepLink;
            var expectedOutput = "ty://?Page=Product&ContentId=1925865";

            Assert.Equal(resultOutput, expectedOutput);
        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_ProductPageWithQueryString_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/casio/erkek-kol-saati-p-1925865?boutiqueId=439892&merchantId=105064" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);

            var resultOutput = result.Data.DeepLink;
            var expectedOutput = "ty://?Page=Product&ContentId=1925865&CampaignId=439892&MerchantId=105064";

            Assert.Equal(resultOutput, expectedOutput);
        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_SearchPageNoQuery_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/tum--urunler?" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);
            
        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_SearchPageValid_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/tum--urunler?q=elbise" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);
            var resultOutput = result.Data.DeepLink;
            var expectedOutput = "ty://?Page=Search&Query=elbise";

            Assert.Equal(resultOutput, expectedOutput);

        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_OtherPagesValid_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/Hesabim/Favoriler" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request, CancellationToken.None);


            Assert.NotNull(result.Data);
            var resultOutput = result.Data.DeepLink;
            var expectedOutput = "ty://?Page=Home";

            Assert.Equal(resultOutput, expectedOutput);

        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_HomePageNoSectionName_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            sctionRepo.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Section, bool>>>())).Returns(LoadSection_Erkek());

            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/butik/liste" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result=  await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);
        }

        [Fact]
        public async void CreateDeepLinkFromUrlHandler_HomePageSectionNameNotExist_Null()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();

            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL = "https://www.trendyol.com/butik/liste/erkek" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result= await sut.Handle(request, CancellationToken.None);


            Assert.Null(result.Data);
        }



        [Fact]
        public async void CreateDeepLinkFromUrlHandler_HomePageValid_Success()
        {

            var requestHistoryRepo = new Mock<IRepository<RequestHistory>>();
            var sctionRepo = new Mock<IRepository<Section>>();
            sctionRepo.Setup(x=>x.GetAsync(It.IsAny<Expression<Func<Section,bool>>>())).Returns(LoadSection_Erkek());

            var logger = new Mock<ILogger<CreateDeepLinkFromUrlHandler>>();
            IWebUrlParserService webUrlParser = new WebUrlParserService(sctionRepo.Object);

            var request = new CreateDeepLinkFromUrlRequest() { WebURL= "https://www.trendyol.com/butik/liste/erkek" };
            var sut = new CreateDeepLinkFromUrlHandler(requestHistoryRepo.Object, logger.Object, webUrlParser);
            var result = await sut.Handle(request,CancellationToken.None);

            Assert.NotNull(result.Data);
        }


       async Task<Section> LoadSection_Erkek()
        {
            var t = Task.Run(() =>
            {
                return
                new Section() {Id=1,Name= "Erkek" };            
            });

            return await t;

        }

      
    }
}
