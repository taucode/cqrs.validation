using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using TauCode.Validation;

namespace TauCode.Cqrs.Validation
{
    public class CodedEntityValidator<TCodedEntityQuery, TCodeValidator> : AbstractValidator<TCodedEntityQuery>
        where TCodedEntityQuery : ICodedEntityQuery
        where TCodeValidator : CodeValidator, new()
    {
        public CodedEntityValidator()
        {
            this.RuleFor(x => x.GetCode())
                .SetValidator(new TCodeValidator())
                .WithName(x => x.GetCodePropertyName());

            this.RuleFor(x => x)
                .Custom(PropertiesCheck);
        }

        private void PropertiesCheck(TCodedEntityQuery query, CustomContext customContext)
        {
            var oneIsValid = query.GetId() == null ^ query.GetCode() == null;
            if (!oneIsValid)
            {
                customContext.AddFailure(new ValidationFailure("query", $"Either '{query.GetIdPropertyName()}' or '{query.GetCodePropertyName()}' must be not null.")
                {
                    ErrorCode = "CodedEntityQueryValidator",
                });
            }
        }
    }
}
