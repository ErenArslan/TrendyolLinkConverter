using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Dtos.Responses;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;

namespace TrendyolLinkConverter.Core.UseCases
{
    public class CreateDeepLinkFromUrlHandler : IRequestHandler<CreateDeepLinkFromUrlRequest, BaseResponseDto<DeepLinkDto>>
    {
        private readonly IRepository<RequestHistory> repository;
        private readonly IWebUrlParserService webUrlParser;
        private readonly ILogger<CreateDeepLinkFromUrlHandler> logger;

        public CreateDeepLinkFromUrlHandler(IRepository<RequestHistory> _repository, ILogger<CreateDeepLinkFromUrlHandler> _logger, IWebUrlParserService _webUrlParser)
        {
            webUrlParser = _webUrlParser;
            repository = _repository;
            logger = _logger;
        }


        public async Task<BaseResponseDto<DeepLinkDto>> Handle(CreateDeepLinkFromUrlRequest request, CancellationToken cancellationToken)
        {
            BaseResponseDto<DeepLinkDto> response = new BaseResponseDto<DeepLinkDto>();

            try
            {
                webUrlParser.SetUrl(request.WebURL);
                var deepLink = await webUrlParser.ConvertToDeepLink();

                if (string.IsNullOrEmpty(deepLink))
                {
                    throw new Exception("Couldn't be created deep link.");
                }

                var requestHistory = new RequestHistory(request.WebURL, deepLink);
              

                await repository.CreateAsync(requestHistory);

                var deepLinkDto = new DeepLinkDto() { DeepLink = deepLink };
                response.Data = deepLinkDto;

            }
            catch (Exception ex)
            {

                response.Errors.Add(ex.Message);
                logger.LogError(ex.Message,"Time: "+DateTime.Now);
            }


            return response;
        }




    }
}
