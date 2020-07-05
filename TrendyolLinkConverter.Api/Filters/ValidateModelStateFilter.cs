using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrendyolLinkConverter.Core.Dtos.Responses;

namespace TrendyolLinkConverter.Api.Filters
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {

                var validationErrors = context.ModelState
                    .Keys
                    .SelectMany(k => context.ModelState[k].Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

              

                context.Result = new BadRequestObjectResult(validationErrors);
                return;
            }
            await next();
        }
    }
}
