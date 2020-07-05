using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Dtos.Responses;
using TrendyolLinkConverter.Core.Exceptions;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;

namespace TrendyolLinkConverter.Core.UseCases
{
   public class GetLinksByShortLinkHandler : IRequestHandler<GetLinksByShortLinkRequest, BaseResponseDto<StoredLinksDto>>
    {
        private readonly IRepository<ShortLink> repository;
        private readonly ILogger<GetLinksByShortLinkHandler> logger;
        private readonly IDistributedCache distributedCache;
        public GetLinksByShortLinkHandler(IDistributedCache _distributedCache,IRepository<ShortLink> _repository, ILogger<GetLinksByShortLinkHandler> _logger)
        {
            distributedCache = _distributedCache;
            repository = _repository;
            logger = _logger;
        }


        public async Task<BaseResponseDto<StoredLinksDto>> Handle(GetLinksByShortLinkRequest request, CancellationToken cancellationToken)
        {
            BaseResponseDto<StoredLinksDto> response = new BaseResponseDto<StoredLinksDto>();

            try
            {

                ShortLink shortLink = null;
                var uri =new Uri(request.ShortLink);

                // path = /asdbcv   remove '/'
                var shortCode = uri.LocalPath.Substring(1);

                var result= await distributedCache.GetStringAsync(shortCode);

                if (!string.IsNullOrEmpty(result))
                {
                    shortLink = JsonConvert.DeserializeObject<ShortLink>(result);
                }
                else
                {
                    shortLink= await repository.GetAsync(p => p.Code == shortCode);
                    if (shortLink != null)
                    {
                       await distributedCache.SetStringAsync(shortLink.Code,JsonConvert.SerializeObject(shortLink));
                    }
                    else
                    {
                        throw new LinkConverterDomainException("Short Link couldn't found.");
                    }
                }

               

                var shortLinkDto = new StoredLinksDto();
                shortLinkDto.DeepLink = shortLink.DeepLink;
                shortLinkDto.WebUrl = shortLink.WebUrl;
                response.Data = shortLinkDto;

            }
            catch (Exception ex)
            {

                response.Errors.Add(ex.Message);
                logger.LogError(ex.Message, "Time: " + DateTime.Now);
            }


            return response;
        }




    }
}
