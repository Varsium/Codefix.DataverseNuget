using Codefix.Dataverse.Core;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Query;
using Codefix.Dataverse.Core.Options;
using System.Text;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities
{
    internal class AddressingEntries<TEntity> : IAddressingEntries<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;

        public AddressingEntries(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataQueryKey<TEntity> ById(params int[] keys)
        {
            _stringBuilder.Append($"{QuerySeparators.LeftBracket}{string.Join(QuerySeparators.StringComma, keys)}{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryKey<TEntity> ById(params string[] keys)
        {
            _stringBuilder.Append($"{QuerySeparators.LeftBracket}'{string.Join($"'{QuerySeparators.Comma}'", keys)}'{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryKey<TEntity> ById(params Guid[] keys)
        {
            _stringBuilder.Append($"{QuerySeparators.LeftBracket}{string.Join(QuerySeparators.StringComma, keys)}{QuerySeparators.RigthBracket}{QuerySeparators.Begin}");

            return new ODataQueryKey<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }

        public IODataQueryCollection<TEntity> ByList()
        {
            _stringBuilder.Append(QuerySeparators.Begin);

            return new ODataQueryCollection<TEntity>(_stringBuilder, _odataQueryBuilderOptions);
        }
    }
}
