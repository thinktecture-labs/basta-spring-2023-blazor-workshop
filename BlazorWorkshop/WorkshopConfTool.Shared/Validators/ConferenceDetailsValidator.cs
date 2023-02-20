using FluentValidation;
using WorkshopConfTool.Shared.Models;

namespace WorkshopConfTool.Shared.Validators
{
    public class ConferenceDetailsValidator : AbstractValidator<ConferenceDetails>
    {
        public ConferenceDetailsValidator()
        {
            RuleFor(conf => conf.DateTo).GreaterThan(conf => conf.DateFrom).WithMessage("Das Datum muss nach dem DateFrom leigen");
        }
    }
}
