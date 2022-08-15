using FluentValidation;

namespace TauCode.Cqrs.Validation;

public interface IQueryValidatorSource
{
    Type[] GetQueryTypes();
    IValidator<TQuery> GetValidator<TQuery>() where TQuery : IQuery;
}