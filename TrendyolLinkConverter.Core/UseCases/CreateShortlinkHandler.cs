using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.DomainEvents.Events;
using TrendyolLinkConverter.Core.Dtos.Requests;
using TrendyolLinkConverter.Core.Dtos.Responses;
using TrendyolLinkConverter.Core.Exceptions;
using TrendyolLinkConverter.Core.Models;
using TrendyolLinkConverter.Core.Repositories;
using TrendyolLinkConverter.Core.Services;

namespace TrendyolLinkConverter.Core.UseCases
{
    public class CreateShortlinkHandler : IRequestHandler<CreateShortLinkRequest, BaseResponseDto<ShortLinkDto>>
    {
        private readonly IRepository<ShortLink> repository;
        private readonly IWebUrlParserService webUrlParser;
        private readonly IDeepLinkParserService deepLinkParser;
        private readonly ILogger<CreateShortlinkHandler> logger;
        private readonly IMediator mediator;

        public CreateShortlinkHandler(IRepository<ShortLink> _repository, IMediator _mediator, ILogger<CreateShortlinkHandler> _logger, IWebUrlParserService _webUrlParser, IDeepLinkParserService _deepLinkParser)
        {
            webUrlParser = _webUrlParser;
            deepLinkParser = _deepLinkParser;
            repository = _repository;
            mediator = _mediator;
            logger = _logger;
        }


        public async Task<BaseResponseDto<ShortLinkDto>> Handle(CreateShortLinkRequest request, CancellationToken cancellationToken)
        {
            BaseResponseDto<ShortLinkDto> response = new BaseResponseDto<ShortLinkDto>();

            try
            {
                ShortLink shortLink = new ShortLink();
                if (!string.IsNullOrEmpty(request.WebURL))
                {
                    shortLink.WebUrl = request.WebURL;
                    webUrlParser.SetUrl(request.WebURL);
                    shortLink.DeepLink = await webUrlParser.ConvertToDeepLink();
                }
                else if(!string.IsNullOrEmpty(request.DeepLink))
                {
                    shortLink.DeepLink = request.DeepLink;
                    deepLinkParser.SetDeepLink(request.DeepLink);
                    shortLink.WebUrl = await deepLinkParser.ConvertToWebUrl();
                }
                else
                {
                    throw new LinkConverterDomainException("At least one input should be applied.");
                }

                shortLink.Code = GetUrl();

                while ( (await repository.GetWhereAsync(p=>p.Code==shortLink.Code)).Any())
                {
                    shortLink.Code = GetUrl();
                }


                await repository.CreateAsync(shortLink);

                var shortLinkDto = new ShortLinkDto() { ShortLink ="http://localhost:8000/"+ shortLink.Code };
                response.Data = shortLinkDto;

                await mediator.Publish(new ShortLinkCreatedEvent(shortLink.Code, shortLink.WebUrl, shortLink.DeepLink));

            }
            catch (Exception ex)
            {

                response.Errors.Add(ex.Message);
                logger.LogError(ex.Message, "Time: " + DateTime.Now);
            }


            return response;
        }



        private string GetUrl()
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }




    }
}
