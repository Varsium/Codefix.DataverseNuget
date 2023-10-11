using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources.Expand;
using Codefix.Dataverse.Core.Conventions.Constants;
using Codefix.Dataverse.Core.Conventions.Functions;
using Codefix.Dataverse.Core.Conventions.Operators;
using Codefix.Dataverse.Core.Expressions.Visitors;
using Codefix.Dataverse.Core.Extensions;
using Codefix.Dataverse.Core.Options;
using System.Linq.Expressions;
using System.Text;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Query
{
    internal class ODataQueryCollection<TEntity> : ODataQuery, IODataQueryCollection<TEntity>
    {
        private bool _hasMultyFilters;
        private bool _hasMultyExpands;

        public ODataQueryCollection(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
            : base(stringBuilder, odataQueryBuilderOptions)
        {
            _hasMultyFilters = false;
            _hasMultyExpands = false;
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryCollection<TEntity> Filter(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false)
        {
            var query = new ODataOptionFilterExpressionVisitor(_odataQueryBuilderOptions).ToQuery(filter, useParenthesis);

            return Filter(query);
        }

        public IODataQueryCollection<TEntity> Expand(Expression<Func<TEntity, object>> expand)
        {
            var query = new ODataOptionExpandExpressionVisitor().ToQuery(expand);

            return Expand(query);
        }

        public IODataQueryCollection<TEntity> Expand(Action<IODataExpandResource<TEntity>> expandNested)
        {
            var builder = new ODataExpandResource<TEntity>(_odataQueryBuilderOptions);

            expandNested(builder);

            return Expand(builder.Query);
        }

        public IODataQueryCollection<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select);

            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByDescending);

            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Skip(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Top(int value)
        {
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Count(bool value = true)
        {
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}{QuerySeparators.Main}");

            return this;
        }

        private IODataQueryCollection<TEntity> Expand<T>(T query) where T : class
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

        private IODataQueryCollection<TEntity> Filter(string query)
        {
            if (_hasMultyFilters)
            {
                _stringBuilder.Merge(ODataOptionNames.Filter, QuerySeparators.Main, $" {ODataLogicalOperations.And} {query}");
            }
            else
            {
                _stringBuilder.Append($"{ODataOptionNames.Filter}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");
            }

            _hasMultyFilters = true;

            return this;
        }

        public Task<IList<TEntity>> ToListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(Guid id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public string ToOdataQuery()
        {
            throw new NotImplementedException();
        }
    }
}
