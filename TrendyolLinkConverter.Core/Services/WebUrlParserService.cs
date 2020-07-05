using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Enums;
using TrendyolLinkConverter.Core.Exceptions;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;

namespace TrendyolLinkConverter.Core.Services
{
    public class WebUrlParserService : IWebUrlParserService
    {
        public string MainUrl { get; private set; }
        public string Protocol { get; private set; }
        public string Host { get; private set; }
        public string Path { get; private set; }
        public Dictionary<string, StringValues> Queries { get; private set; }

        readonly IDictionary<PageType, Func<Task<string>>> pageTypeHandlers;
        readonly IRepository<Section> repository;
        public WebUrlParserService(IRepository<Section> _repository)
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


        public void SetUrl(string mainUrl)
        {
            if ( new Regex(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$").Match(mainUrl).Success)
            {
                MainUrl = mainUrl;
                Uri uri = new Uri(MainUrl);
                Protocol = uri.Scheme;
                Host = uri.Host;
                Path = uri.LocalPath;
                Queries = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            }
            else
            {
                throw new LinkConverterDomainException("Invalid URL");
            }
        }


        public async Task<string> ConvertToDeepLink()
        {
            if (string.IsNullOrEmpty(MainUrl))
            {
                throw new LinkConverterDomainException("URL is not exist!");
            }
            return await pageTypeHandlers[Matcher(Path)].Invoke();
        }

        private async Task<string> CreateHomePageHandler()
        {
         
            string deepLink = "ty://?Page=Home";


            string sectionName = Path.Remove(0, "/butik/liste".Length);

            if (sectionName.Length < 1)
            {
                throw new LinkConverterDomainException("Invalid URL");
            }

            sectionName = sectionName.Substring(1);

            //If there is beutified names We can compare directly.
            var section = await repository.GetAsync(p => sectionName.ToUpper().Equals(p.Name.ToUpper()));
            if (section == null)
            {
                throw new LinkConverterDomainException("Section is not exist");
            }

            deepLink += "&SectionId=" + section.Id;


            return deepLink;
        }
        private async Task<string> CreateProductPageHandler()
        {
           var t= Task.Run(() =>
            {

                string deepLink = "ty://?Page=Product";
                var contentId = Path.Substring(Path.IndexOf("-p-") + 3);
                if (string.IsNullOrEmpty(contentId))
                {
                    throw new LinkConverterDomainException("Content Id not found");
                }
                deepLink += "&ContentId=" + contentId;

                if (Queries.Keys.Where(p => p == "boutiqueId").Any())
                {
                    deepLink += "&CampaignId=" + Queries["boutiqueId"];
                }

                if (Queries.Keys.Where(p => p == "merchantId").Any())
                {
                    deepLink += "&MerchantId=" + Queries["merchantId"];
                }


                return deepLink;
            });

            return await t;
        }
        private async Task<string> CreateSearchPageHandler()
        {
            var t = Task.Run(() => 
            {
                string deepLink = "ty://?Page=Search";

                if (Queries.Keys.Any(p => p == "q"))
                {
                    deepLink += "&Query=" + Queries["q"];
                }
                else
                {
                    throw new LinkConverterDomainException("Query cannot be empty.");

                }


                return deepLink;
            });

            return await t;
        }

        private async Task<string> CreateOtherPagesHandler()
        {
            var t = Task.Run(() =>
            {
                string deepLink = "ty://?Page=Home";

                return deepLink;
            });

            return await t;
        }

        public PageType Matcher(string command)
        {

            if (new Regex(@"^[""/]\b(butik)[""/]\b(liste)").Match(command).Success)
            {
                return PageType.HomePage;
            }
            else if (new Regex(@"\b(-p-)+").Match(command).Success)
            {
                return PageType.ProductPage;
            }
            else if (new Regex(@"^[""/]\b(tum--urunler)").Match(command).Success)
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
