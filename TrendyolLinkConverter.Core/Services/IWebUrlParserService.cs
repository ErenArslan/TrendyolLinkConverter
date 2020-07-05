using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrendyolLinkConverter.Core.Services
{
   public interface IWebUrlParserService
    {
        Task<string> ConvertToDeepLink();
        void SetUrl(string mainUrl);
    }
}
