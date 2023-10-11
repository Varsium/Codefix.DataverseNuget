using Codefix.Dataverse.Core.Conventions.AddressingEntities;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Options;
using System;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Query
{
    public interface IODataQueryKey<TEntity> : IODataOptionKey<IODataQueryKey<TEntity>, TEntity>, IODataQuery
    {
        public IAddressingEntries<TResource> For<TResource>(Expression<Func<TEntity, object>> resource);
    }
}
