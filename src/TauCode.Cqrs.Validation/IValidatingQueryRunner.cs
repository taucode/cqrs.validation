using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Abstractions;
using TauCode.Cqrs.Queries;

namespace TauCode.Cqrs.Validation
{
    public interface IValidatingQueryRunner : IQueryRunner
    {
        void Validate<TQuery>(TQuery query) where TQuery : IQuery;

        Task ValidateAsync<TQuery>(TQuery query, CancellationToken cancellationToken) where TQuery : IQuery;
    }
}
