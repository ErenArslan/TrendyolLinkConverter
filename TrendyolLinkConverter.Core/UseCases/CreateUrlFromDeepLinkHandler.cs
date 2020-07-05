using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Dtos.Responses;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;

namespace TrendyolLinkConverter.Core.UseCases
{
   public class CreateUrlFromDeepLinkHandler : IRequestHandler<CreateUrlFromDeepLinkRequest, BaseResponseDto<WebUrlDto>>
    {
        private readonly IRepository<RequestHistory> repository;
        private readonly IDeepLinkParserService deepLinkParser;
        private readonly ILogger<CreateUrlFromDeepLinkHandler> logger;

        public CreateUrlFromDeepLinkHandler(IRepository<RequestHistory> _repository, ILogger<CreateUrlFromDeepLinkHandler> _logger, IDeepLinkParserService _deepLinkParser)
        {
            deepLinkParser = _deepLinkParser;
            repository = _repository;
            logger = _logger;
        }


        public async Task<BaseResponseDto<WebUrlDto>> Handle(CreateUrlFromDeepLinkRequest request, CancellationToken cancellationToken)
        {
            BaseResponseDto<WebUrlDto> response = new BaseResponseDto<WebUrlDto>();

            try
            {
                deepLinkParser.SetDeepLink(request.DeepLink);
                var webUrl = await deepLinkParser.ConvertToWebUrl();

                if (string.IsNullOrEmpty(webUrl))
                {
                    throw new Exception("Couldn't be created web url.");
                }

                var requestHistory = new RequestHistory(request.DeepLink, webUrl);


                await repository.CreateAsync(requestHistory);

                var webUrlDto = new WebUrlDto() { WebUrl = webUrl };
                response.Data = webUrlDto;

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
