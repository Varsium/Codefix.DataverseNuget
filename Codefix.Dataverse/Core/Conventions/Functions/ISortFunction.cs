namespace Codefix.Dataverse.Core.Conventions.Functions
{
    /// <summary>
    /// Sort functions
    /// </summary>
    public interface ISortFunction
    {
        /// <summary>
        /// Sort ascending
        /// </summary>
        ISortFunction Ascending<T>(T column);

        /// <summary>
        /// Sort descending
        /// </summary>
        ISortFunction Descending<T>(T column);
    }
}
