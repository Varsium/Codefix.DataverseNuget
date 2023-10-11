namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources
{
    internal interface IODataResource
    {
        IAddressingEntries<TEntity> For<TEntity>(string resource);
    }
}
