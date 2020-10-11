using FluentValidation;
using System;
using TauCode.Cqrs.Abstractions;

namespace TauCode.Cqrs.Validation
{
    public interface IQueryValidatorSource
    {
        Type[] GetQueryTypes();
        IValidator<TQuery> GetValidator<TQuery>() where TQuery : IQuery;
    }
}
