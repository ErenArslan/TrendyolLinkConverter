using FluentValidation;
using TrendyolLinkConverter.Core.Dtos.Requests;

namespace TrendyolLinkConverter.Core.Validations
{
   public class CreateDeepLinkFromUrlValidation : AbstractValidator<CreateDeepLinkFromUrlRequest>
    {
        public CreateDeepLinkFromUrlValidation()
        {
            RuleFor(x => x.WebURL)
                .NotNull()
                .NotEmpty()
                .WithMessage("WebUrl boş olamaz.");
        }
    }



}
