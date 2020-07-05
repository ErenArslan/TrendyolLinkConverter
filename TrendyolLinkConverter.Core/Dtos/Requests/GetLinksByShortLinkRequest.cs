using MediatR;
using System;
using TrendyolLinkConverter.Core.Dtos.Responses;

namespace TrendyolLinkConverter.Core.Dtos.Requests
{
    public class GetLinksByShortLinkRequest : IRequest<BaseResponseDto<StoredLinksDto>>
    {
        public string ShortLink { get; set; }
    }
}
