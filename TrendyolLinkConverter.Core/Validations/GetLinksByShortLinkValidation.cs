using FluentValidation;
using TrendyolLinkConverter.Core.Dtos.Requests;

namespace TrendyolLinkConverter.Core.Validations
{
    public  class GetLinksByShortLinkValidation : AbstractValidator<GetLinksByShortLinkRequest>
    {
        public GetLinksByShortLinkValidation()
        {
            RuleFor(x => x.ShortLink)
                .NotNull()
                .NotEmpty()
                .WithMessage("ShortLink cannot be empty!");
        }
    }



}