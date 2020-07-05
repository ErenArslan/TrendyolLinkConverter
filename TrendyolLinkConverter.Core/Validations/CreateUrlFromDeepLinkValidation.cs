using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TrendyolLinkConverter.Core.Dtos.Requests;

namespace TrendyolLinkConverter.Core.Validations
{
   public class CreateUrlFromDeepLinkValidation : AbstractValidator<CreateUrlFromDeepLinkRequest>
    {
        public CreateUrlFromDeepLinkValidation()
        {
            RuleFor(x => x.DeepLink)
                .NotNull()
                .NotEmpty()
                .WithMessage("DeepLink cannot be blank!.");
        }
    }



}
