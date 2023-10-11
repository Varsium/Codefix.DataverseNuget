namespace Codefix.Dataverse.Core.Conventions.Functions
{
    /// <summary>
    /// OData functions
    /// </summary>
    public interface IODataFunction : IODataStringAndCollectionFunction, IODataDateTimeFunction, ICustomFunction, ITypeFunction
    {
    }
}
