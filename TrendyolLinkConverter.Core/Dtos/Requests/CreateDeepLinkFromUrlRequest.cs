using MediatR;
using TrendyolLinkConverter.Core.Dtos.Responses;

namespace TrendyolLinkConverter.Core.Dtos.Requests
{
   public class CreateDeepLinkFromUrlRequest : IRequest<BaseResponseDto<DeepLinkDto>>
    {
        public string WebURL { get; set; }
    }
}
