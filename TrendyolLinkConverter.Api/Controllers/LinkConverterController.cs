﻿using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Requests;

namespace TrendyolLinkConverter.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LinkConverterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LinkConverterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateDeepLinkFromUrl")]
        public async Task<ActionResult<string>> CreateDeepLinkFromUrl([FromBody]CreateDeepLinkFromUrlRequest request)
        {
            var response  = await _mediator.Send(request);

            if (!response.HasError)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Errors);
            }
        }

    }
}