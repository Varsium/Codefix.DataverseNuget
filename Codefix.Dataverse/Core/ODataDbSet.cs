using Codefix.Dataverse.Core.Conventions.AddressingEntities.Query;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources.Expand;
using Codefix.Dataverse.Core.Conventions.Constants;
using Codefix.Dataverse.Core.Conventions.Functions;
using Codefix.Dataverse.Core.Conventions.Operators;
using Codefix.Dataverse.Core.Expressions.Visitors;
using Codefix.Dataverse.Core.Extensions;
using Codefix.Dataverse.Core.Options;
using Codefix.Dataverse.Extensions;
using Codefix.Dataverse.Services;
using System.Linq.Expressions;
using System.Text;

namespace Codefix.Dataverse.Core
{
    public sealed class ODataDbSet<TEntity> : IODataQueryCollection<TEntity> where TEntity : class
    {
        private DataverseService _service;
        internal Type ElementType => typeof(TEntity);
        private bool _hasMultyFilters;
        private bool _hasMultyExpands;
        private readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        private readonly StringBuilder _stringBuilder;
        public ODataDbSet(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions, ref DataverseService dataverse)
        {
            _service = dataverse;
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
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
        private IODataQueryCollection<TEntity> Filter(string query)
        {
            if (_stringBuilder.ToString().Contains(query))
            {
                return this;
            }
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
        public IODataQueryCollection<TEntity> Where(Expression<Func<TEntity, bool>> filter, bool useParenthesis = false)
        {
            return Filter(filter, useParenthesis);
        }

        public IODataQueryCollection<TEntity> Where(Expression<Func<TEntity, IODataFunction, bool>> filter, bool useParenthesis = false)
        {
            return Filter(filter, useParenthesis);
        }

        public IODataQueryCollection<TEntity> Where(Expression<Func<TEntity, IODataFunction, IODataOperator, bool>> filter, bool useParenthesis = false)
        {
            return Filter(filter, useParenthesis);
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
        private IODataQueryCollection<TEntity> Expand<T>(T query) where T : class
        {
            if (_stringBuilder.ToString().Contains(query.ToString()))
            {
                return this;
            }
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
        public IODataQueryCollection<TEntity> Include(Expression<Func<TEntity, object>> expand)
        {
            return Expand(expand);
        }

        public IODataQueryCollection<TEntity> Include(Action<IODataExpandResource<TEntity>> expandNested)
        {
            return Expand(expandNested);
        }

        public IODataQueryCollection<TEntity> Select(Expression<Func<TEntity, object>> select)
        {
            var query = new ODataOptionSelectExpressionVisitor().ToQuery(select);
            if (_stringBuilder.ToString().Contains(query))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.Select}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy);
            if (_stringBuilder.ToString().Contains(query))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Asc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderBy(Expression<Func<TEntity, ISortFunction, object>> orderBy)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderBy);
            if (_stringBuilder.ToString().Contains(query))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            var query = new ODataOptionOrderByExpressionVisitor().ToQuery(orderByDescending);
            if (_stringBuilder.ToString().Contains(query))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.OrderBy}{QuerySeparators.EqualSign}{query} {QuerySorts.Desc}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Skip(int value)
        {
            if (_stringBuilder.ToString().Contains($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}"))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.Skip}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Top(int value)
        {
            if (_stringBuilder.ToString().Contains($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}"))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.Top}{QuerySeparators.EqualSign}{value}{QuerySeparators.Main}");

            return this;
        }

        public IODataQueryCollection<TEntity> Take(int value)
        {
            return Top(value);
        }
        public IODataQueryCollection<TEntity> Count(bool value = true)
        {
            if (_stringBuilder.ToString().Contains($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}"))
            {
                return this;
            }
            _stringBuilder.Append($"{ODataOptionNames.Count}{QuerySeparators.EqualSign}{value.ToString().ToLowerInvariant()}{QuerySeparators.Main}");

            return this;
        }


        public async Task<IList<TEntity>> ToListAsync()
        {
            var result = await _service.GetEntityInDataverse<List<TEntity>>(ElementType.GetTableName(), _stringBuilder.ToString());

            SetBaseValues();
            return result;
        }
        public string ToOdataQuery()
        {
            return _stringBuilder.ToString();
        }
        public async Task<TEntity> FirstOrDefaultAsync()
        {
            var result = await _service.GetEntityInDataverse<TEntity>(ElementType.GetTableName(), _stringBuilder.ToString());
            SetBaseValues();
            return result;
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var result = await _service.PostEntityInDataverse(ElementType.GetTableName(), entity);
            return result;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _service.DeleteEntityInDataverse<TEntity>($"{typeof(TEntity).GetTableName()}({id})");
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var resultP = await _service.PatchEntityInDataverse($"{ElementType.GetTableName()}({entity.GetPrimaryKey()})", entity);

            return resultP;
        }

        public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
        {
            var resultP = await _service.PatchEntityInDataverse($"{ElementType.GetTableName()}({id})", entity);

            return resultP;
        }
        public async Task<TEntity> FirstOrDefaultAsync(Guid id)
        {
            var result = await _service.GetEntityInDataverse<TEntity>($"{ElementType.GetTableName()}({id})", _stringBuilder.ToString());
            SetBaseValues();
            return result;
        }

        internal void SetBaseValues()
        {
            _hasMultyExpands = false;
            _hasMultyFilters = false;
            _stringBuilder.Clear();

        }


    }
}
