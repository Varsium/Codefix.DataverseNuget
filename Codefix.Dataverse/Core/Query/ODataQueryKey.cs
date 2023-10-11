using Codefix.Dataverse.Core.Conventions.AddressingEntities;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources.Expand;
using Codefix.Dataverse.Core.Conventions.Constants;
using Codefix.Dataverse.Core.Expressions.Visitors;
using Codefix.Dataverse.Core.Extensions;
using Codefix.Dataverse.Core.Options;
using System.Linq.Expressions;
using System.Text;

namespace Codefix.Dataverse.Core.Query
{
    internal class ODataQueryKey<TEntity> : ODataQuery, IODataQueryKey<TEntity>
    {
        private bool _hasMultyExpands;

        public ODataQueryKey(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
            _hasMultyExpands = false;
        }

        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource)
        {
            _stringBuilder.LastReplace(QuerySeparators.Begin, QuerySeparators.Slash);

            return new ODataResource<TEntity>(_stringBuilder, _odataQueryBuilderOptions).For<TResource>(resource);
        }

        public IODataQueryKey<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expand);

            return Expand(query);
        }

        public IODataQueryKey<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            return Expand(builder.Query);
        }

        public IODataQueryKey<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        private IODataQueryKey<TEntity> Expand<T>(T query) where T : class
        {
            if (_hasMultyExpands)
            {
                _stringBuilder.Merge(ODataOptionNames.Expand, QuerySeparators.Main, $"{QuerySeparators.Comma}{query}");
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Expand}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasMultyExpands = true;

            return this;
        }
    }
}
