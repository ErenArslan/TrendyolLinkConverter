using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Enums;
using TrendyolLinkConverter.Core.Exceptions;
using TrendyolLinkConverter.Core.Extensions;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;

namespace TrendyolLinkConverter.Core.Services
{
    public class DeepLinkParserService : IDeepLinkParserService
    {
        public string DeepLink { get; private set; }
        readonly IDictionary<PageType, Func<Task<string>>> pageTypeHandlers;
        readonly IRepository<Section> repository;

        public DeepLinkParserService(IRepository<Section> _repository)
        {
            repository = _repository;
            pageTypeHandlers = new Dictionary<PageType, Func<Task<string>>>
            {
                 {PageType.HomePage, CreateHomePageHandler},
                 {PageType.ProductPage, CreateProductPageHandler},
                 {PageType.SearchPage, CreateSearchPageHandler},
                 {PageType.OtherPages, CreateOtherPagesHandler}
            };
        }
        public async Task<string> ConvertToWebUrl()
        {
            if (string.IsNullOrEmpty(DeepLink))
            {
                throw new LinkConverterDomainException("Deep Link is not exist!");
            }
            return await pageTypeHandlers[Matcher()].Invoke();
        }

        public void SetDeepLink(string deepLink)
        {
            if (new Regex(@"^\b(ty:\/\/\?)\b(Page=)\b([a-zA-Z])").Match(deepLink).Success == false)
            {
                throw new LinkConverterDomainException("Invalid deep link");
            }

            DeepLink = deepLink;

        }

        private async Task<string> CreateHomePageHandler()
        {

            string url = "https://www.trendyol.com/butik/liste/";

            var sectionIdCursorIndex = DeepLink.IndexOf("SectionId");
            if (sectionIdCursorIndex < 0)
            {
                throw new LinkConverterDomainException("Section Id is not exist");
            }
            //index + 'sectionId=' length
            sectionIdCursorIndex += 10;

            string idString = DeepLink.Substring(sectionIdCursorIndex);
            int sectionId = 0;
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out sectionId))
            {
                throw new LinkConverterDomainException("Section Id is not exist");
            }


            var section = await repository.GetAsync(p => p.Id == sectionId);
            if (section == null)
            {
                throw new LinkConverterDomainException("Section is not exist");
            }

            url += section.Name.ToLower().ConvertFromTurkishCharacters();

            return url;
        }
        private async Task<string> CreateProductPageHandler()
        {
            var t = Task.Run(() =>
            {

                string url = "https://www.trendyol.com/brand/name-p-";

                var contentIdCursorIndex = DeepLink.IndexOf("ContentId");
                if (contentIdCursorIndex < 0)
                {
                    throw new LinkConverterDomainException("Content Id is not exist");
                }

                //index + 'ContentId=' length
                contentIdCursorIndex += 10;


                var contentIdEndIndex = DeepLink.IndexOf("&", contentIdCursorIndex) > -1 ?
                DeepLink.IndexOf("&", contentIdCursorIndex) :
                DeepLink.Length;

                var contentId = DeepLink.Substring(contentIdCursorIndex, contentIdEndIndex - contentIdCursorIndex);

                if (string.IsNullOrEmpty(contentId))
                {
                    throw new LinkConverterDomainException("Content Id is not exist");
                }
                url += contentId;


                bool isFirstQuery = true;
                var campaignIdCursorIndex = DeepLink.IndexOf("CampaignId");
                if (campaignIdCursorIndex > -1)
                {
                    //index + 'ContentId=' length
                    campaignIdCursorIndex += 11;
                    var campaignIdEndIndex = DeepLink.IndexOf("&", campaignIdCursorIndex) > -1 ?
                    DeepLink.IndexOf("&", campaignIdCursorIndex) :
                    DeepLink.Length;


                    var campaignId = DeepLink.Substring(campaignIdCursorIndex, campaignIdEndIndex - campaignIdCursorIndex);

                    if (!string.IsNullOrEmpty(campaignId))
                    {
                        url += isFirstQuery ? "?" : "&";
                        isFirstQuery = false;
                        url += "boutiqueId=" + campaignId;
                       
                    }
                    
                }

                var merchantIdCursorIndex = DeepLink.IndexOf("MerchantId");
                if (merchantIdCursorIndex > -1)
                {
                    //index + 'merchantId=' length
                    merchantIdCursorIndex += 11;
                    var merchantIdEndIndex = DeepLink.IndexOf("&", merchantIdCursorIndex) > -1 ?
                    DeepLink.IndexOf("&", merchantIdCursorIndex) :
                    DeepLink.Length;


                    var merchantId = DeepLink.Substring(merchantIdCursorIndex, merchantIdEndIndex - merchantIdCursorIndex);

                    if (!string.IsNullOrEmpty(merchantId))
                    {
                        url += isFirstQuery ? "?" : "&";
                        isFirstQuery = false;
                        url += "merchantId=" + merchantId;
                    }

                }


                return url;
            });

            return await t;
        }
        private async Task<string> CreateSearchPageHandler()
        {
            var t = Task.Run(() =>
            {
                string url = "https://www.trendyol.com/tum--urunler?q=";

                var queryCursorIndex = DeepLink.IndexOf("Query");
                if (queryCursorIndex> -1)
                {
                    // queryCursorIndex + 'Query=' length
                    queryCursorIndex += 6;

                    var query = DeepLink.Substring(queryCursorIndex);
                    url +=  query;
                }
                else
                {
                    throw new LinkConverterDomainException("Query cannot be empty.");

                }


                return url;
            });

            return await t;
        }

        private async Task<string> CreateOtherPagesHandler()
        {
            var t = Task.Run(() =>
            {
                string url = "https://www.trendyol.com";

                return url;
            });

            return await t;
        }

        public PageType Matcher()
        {

            if (new Regex(@"^\b(ty:\/\/\?)\b(Page=)\b(Home)").Match(DeepLink).Success)
            {
                return PageType.HomePage;
            }
            else if (new Regex(@"^\b(ty:\/\/\?)\b(Page=)\b(Product)").Match(DeepLink).Success)
            {
                return PageType.ProductPage;
            }
            else if (new Regex(@"^\b(ty:\/\/\?)\b(Page=)\b(Search)").Match(DeepLink).Success)
            {
                return PageType.SearchPage;
            }
            else
            {
                return PageType.OtherPages;
            }


        }

    }
}
