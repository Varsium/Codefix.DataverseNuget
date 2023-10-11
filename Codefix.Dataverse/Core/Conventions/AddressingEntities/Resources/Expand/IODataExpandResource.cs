using Codefix.Dataverse.Core.Conventions.AddressingEntities.Query.Expand;
using System;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources.Expand
{
    public interface IODataExpandResource<TEntity>
    {
        IODataQueryExpand<TExpandNestedEntity> For<TExpandNestedEntity>(Expression<Func<TEntity, object>> expandNested);
    }
}
