using System;
using System.Linq.Expressions;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources
{
    internal interface IODataResource<TResource>
    {
        IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource);
    }
}
