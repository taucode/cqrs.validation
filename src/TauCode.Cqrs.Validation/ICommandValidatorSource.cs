using FluentValidation;

namespace TauCode.Cqrs.Validation;

public interface ICommandValidatorSource
{
    Type[] GetCommandTypes();
    IValidator<TCommand> GetValidator<TCommand>() where TCommand : ICommand;
}