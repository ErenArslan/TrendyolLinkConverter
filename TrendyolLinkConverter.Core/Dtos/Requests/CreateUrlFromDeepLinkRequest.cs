using MediatR;
using TrendyolLinkConverter.Core.Dtos.Responses;

namespace TrendyolLinkConverter.Core.Dtos.Requests
{
    public class CreateUrlFromDeepLinkRequest : IRequest<BaseResponseDto<WebUrlDto>>
    {
        public string DeepLink { get; set; }
    }
}
