using Codefix.Dataverse.Core.Conventions.AddressingEntities.Options;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Query.Expand
{
    public interface IODataQueryExpand<TEntity> : IODataOptionCollection<IODataQueryExpand<TEntity>, TEntity>
    {
    }
}
