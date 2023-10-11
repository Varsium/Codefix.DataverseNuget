using Codefix.Dataverse.Core.Conventions.AddressingEntities.Query.Expand;
using Codefix.Dataverse.Core.Expressions.Visitors;
using Codefix.Dataverse.Core.Options;
using System.Linq.Expressions;
using System.Text;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources.Expand
{
    internal class ODataExpandResource<TEntity> : IODataExpandResource<TEntity>
    {
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        private AbstractODataQueryExpand _odataQueryExpand;

        public StringBuilder Query
        {
            get
            {
                if (_odataQueryExpand?.Query?.Length > 0)
                {
                    return _stringBuilder.Append($"{QuerySeparators.LeftBracket}{_odataQueryExpand.Query}{QuerySeparators.RigthBracket}");
                }

                return _stringBuilder;
            }
        }

        public ODataExpandResource(ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = new StringBuilder();
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public IODataQueryExpand<TNestedEntity> For<TNestedEntity>(Expression<Func<TEntity, object>> nestedExpand)
        {
            var query = new ODataResourceExpressionVisitor().ToQuery(nestedExpand);

            if (_odataQueryExpand?.Query?.Length > 0)
            {
                _stringBuilder.Append($"{QuerySeparators.LeftBracket}{_odataQueryExpand.Query}{QuerySeparators.RigthBracket}{QuerySeparators.Comma}{query}");
            }
            else
            {
                if (query != default)
                {
                    if (_stringBuilder.Length > 0)
                    {
                        _stringBuilder.Append(QuerySeparators.Comma);
                    }

                    _stringBuilder.Append(query);
                }
            }

            _odataQueryExpand = new ODataQueryExpand<TNestedEntity>(_odataQueryBuilderOptions);

            return _odataQueryExpand as ODataQueryExpand<TNestedEntity>;
        }
    }
}
