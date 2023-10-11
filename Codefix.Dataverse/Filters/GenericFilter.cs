using Codefix.Dataverse.Extensions;

namespace Codefix.Dataverse.Filters
{
    public record GenericFilter<TEntity> where TEntity : class
    {
        public string Query { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public string GetTableName()
        {
            return typeof(TEntity).GetTableName();
        }
    }
    public class GenericClassFilter<TEntity> where TEntity : class
    {
        public string Query { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public string GetTableName()
        {
            return typeof(TEntity).GetTableName();
        }
    }

}
