using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TauCode.Cqrs.Abstractions;

namespace TauCode.Cqrs.Validation
{
    public class QueryValidatorSource : IQueryValidatorSource
    {
        #region Constants

        private static readonly object[] EmptyArgs = { };

        #endregion

        #region Fields

        /// <summary>
        /// Key is query type, Value is query validator constructor
        /// </summary>
        private readonly Dictionary<Type, ConstructorInfo> _queryValidatorConstructors;


        #endregion

        #region Constructor

        public QueryValidatorSource(Assembly validatorsAssembly)
        {
            if (validatorsAssembly == null)
            {
                throw new ArgumentNullException(nameof(validatorsAssembly));
            }

            _queryValidatorConstructors = validatorsAssembly
                .GetTypes()
                .Select(GetQueryValidatorInfo)
                .Where(x => x != null)
                .ToDictionary(x => x.Item1, x => x.Item2);
        }

        #endregion

        #region Private

        private static Tuple<Type, ConstructorInfo> GetQueryValidatorInfo(Type supposedQueryValidatorType)
        {
            var type = supposedQueryValidatorType; // lazy

            var interfaces = type.GetInterfaces();

            // search for IValidator<TQuery> where TQuery: IQuery
            foreach (var @interface in interfaces)
            {
                var isGeneric = @interface.IsConstructedGenericType;
                if (!isGeneric)
                {
                    continue;
                }

                var getGenericTypeDefinition = @interface.GetGenericTypeDefinition();
                if (getGenericTypeDefinition != typeof(IValidator<>))
                {
                    continue;
                }

                var supposedQueryType = @interface.GetGenericArguments().Single();
                var argInterfaces = supposedQueryType.GetInterfaces();
                if (argInterfaces.Contains(typeof(IQuery)))
                {
                    var constructor = type.GetConstructor(Type.EmptyTypes);
                    if (constructor == null)
                    {
                        throw new ArgumentException($"Type '{type.FullName}' does not have a parameterless constructor.");
                    }

                    return Tuple.Create(supposedQueryType, constructor);
                }
            }

            return null;
        }

        #endregion

        #region IQueryValidatorSource Members

        public Type[] GetQueryTypes() => _queryValidatorConstructors.Keys.ToArray();

        public IValidator<TQuery> GetValidator<TQuery>() where TQuery : IQuery
        {
            _queryValidatorConstructors.TryGetValue(typeof(TQuery), out var ctor);
            if (ctor == null)
            {
                return null;
            }

            var validator = ctor.Invoke(EmptyArgs);
            return (IValidator<TQuery>)validator;
        }

        #endregion
    }
}
