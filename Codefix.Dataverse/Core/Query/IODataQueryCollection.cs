using Codefix.Dataverse.Core.Conventions.AddressingEntities.Options;

namespace Codefix.Dataverse.Core.Query
{
    public interface IODataQueryCollection<TEntity> : IODataOptionCollection<IODataQueryCollection<TEntity>, TEntity>, IODataQuery
    {
    }
}