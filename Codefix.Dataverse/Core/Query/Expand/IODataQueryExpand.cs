using Codefix.Dataverse.Core.Conventions.AddressingEntities.Options;

namespace Codefix.Dataverse.Core.Query.Expand
{
    public interface IODataQueryExpand<TEntity> : IODataOptionCollection<IODataQueryExpand<TEntity>, TEntity>
    {
    }
}
