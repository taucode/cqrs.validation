using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Abstractions;
using TauCode.Cqrs.Commands;

namespace TauCode.Cqrs.Validation
{
    public interface IValidatingCommandDispatcher : ICommandDispatcher
    {
        void Validate<TCommand>(TCommand command) where TCommand : ICommand;

        Task ValidateAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand;
    }
}
