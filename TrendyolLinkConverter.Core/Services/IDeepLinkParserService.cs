using System.Threading.Tasks;

namespace TrendyolLinkConverter.Core.Services
{
    public interface IDeepLinkParserService
    {
        Task<string> ConvertToWebUrl();
        void SetDeepLink(string deepLink);
    }
}
