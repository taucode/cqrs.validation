using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Abstractions;
using TauCode.Cqrs.Queries;

namespace TauCode.Cqrs.Validation
{
    public class ValidatingQueryRunner : QueryRunner, IValidatingQueryRunner
    {
        #region Constructor

        public ValidatingQueryRunner(
            IQueryHandlerFactory queryHandlerFactory,
            IQueryValidatorSource queryValidatorSource)
            : base(queryHandlerFactory)
        {
            this.QueryValidatorSource =
                queryValidatorSource
                ??
                throw new ArgumentNullException(nameof(queryValidatorSource));
        }


        #endregion

        #region Protected

        protected IQueryValidatorSource QueryValidatorSource { get; }

        #endregion

        #region Overridden

        protected override void OnBeforeExecuteHandler<TQuery>(IQueryHandler<TQuery> handler, TQuery query)
        {
            this.Validate(query);
        }

        protected override Task OnBeforeExecuteHandlerAsync<TQuery>(IQueryHandler<TQuery> handler, TQuery query, CancellationToken cancellationToken)
        {
            return this.ValidateAsync(query, cancellationToken);
        }


        #endregion

        #region IValidatingQueryRunner Members

        public void Validate<TQuery>(TQuery query) where TQuery : IQuery
        {
            var validator = this.QueryValidatorSource.GetValidator<TQuery>();
            if (validator != null)
            {
                var validationResult = validator.Validate(query);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The query is invalid.", validationResult.Errors);
                }
            }
        }

        public async Task ValidateAsync<TQuery>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery
        {
            var validator = this.QueryValidatorSource.GetValidator<TQuery>();
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(query, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException("The query is invalid.", validationResult.Errors);
                }
            }
        }

        #endregion
    }
}