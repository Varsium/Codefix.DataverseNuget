namespace Codefix.Dataverse.Core
{
    public interface IODataQueryable<T>
    {
        internal Task<IList<T>> ToListAsync();
        internal Task<T> FirstOrDefaultAsync();
    }
}
