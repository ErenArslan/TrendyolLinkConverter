using MediatR;
using TrendyolLinkConverter.Core.Dtos.Responses;

namespace TrendyolLinkConverter.Core.Dtos.Requests
{
   public class CreateShortLinkRequest : IRequest<BaseResponseDto<ShortLinkDto>>
    {
        public string WebURL { get; set; }
        public string DeepLink { get; set; }

    }
}
