using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Abstractions;
using TauCode.Cqrs.Commands;

namespace TauCode.Cqrs.Validation
{
    public class ValidatingCommandDispatcher : CommandDispatcher, IValidatingCommandDispatcher
    {
        #region Constructor

        public ValidatingCommandDispatcher(
            ICommandHandlerFactory commandHandlerFactory,
            ICommandValidatorSource commandValidatorSource)
            : base(commandHandlerFactory)
        {
            this.CommandValidatorSource =
                commandValidatorSource
                ??
                throw new ArgumentNullException(nameof(commandValidatorSource));
        }


        #endregion

        #region Protected

        protected ICommandValidatorSource CommandValidatorSource { get; }

        #endregion

        #region Overridden

        protected override void OnBeforeExecuteHandler<TCommand>(ICommandHandler<TCommand> handler, TCommand command)
        {
            this.Validate(command);
        }

        protected override Task OnBeforeExecuteHandlerAsync<TCommand>(
            ICommandHandler<TCommand> handler,
            TCommand command,
            CancellationToken cancellationToken)
        {
            return this.ValidateAsync(command, cancellationToken);
        }


        #endregion

        #region IValidatingCommandDispatcher Members

        public void Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            var validator = this.CommandValidatorSource.GetValidator<TCommand>();
            if (validator != null)
            {
                var validationResult = validator.Validate(command);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The command is invalid.", validationResult.Errors);
                }
            }
        }

        public async Task ValidateAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
        {
            var validator = this.CommandValidatorSource.GetValidator<TCommand>();
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The command is invalid.", validationResult.Errors);
                }
            }
        }


        #endregion
    }
}