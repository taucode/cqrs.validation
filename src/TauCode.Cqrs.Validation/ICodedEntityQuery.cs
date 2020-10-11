using TauCode.Cqrs.Abstractions;
using TauCode.Domain.Identities;

namespace TauCode.Cqrs.Validation
{
    public interface ICodedEntityQuery : IQuery
    {
        IdBase GetId();
        string GetCode();
        string GetIdPropertyName();
        string GetCodePropertyName();
    }
}
