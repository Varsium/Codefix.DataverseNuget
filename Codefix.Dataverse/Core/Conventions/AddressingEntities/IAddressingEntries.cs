using Codefix.Dataverse.Core.Conventions.AddressingEntities.Query;
using System;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities
{
    public interface IAddressingEntries<TEntity>
    {
        IODataQueryKey<TEntity> ById(params int[] keys);

        IODataQueryKey<TEntity> ById(params string[] keys);

        IODataQueryKey<TEntity> ById(params Guid[] keys);

        IODataQueryCollection<TEntity> ByList();
    }
}
