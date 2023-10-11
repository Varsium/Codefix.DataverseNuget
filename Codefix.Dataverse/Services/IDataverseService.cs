namespace Codefix.Dataverse.Services
{
    public interface IDataverseService
    {
        Task<T> ConfigureDataverseRequestAsync<T>(HttpMethod httpMethod, string table, string filter = null, object customClass = null) where T : class;
        Task<bool> DeleteEntityInDataverse<T>(string tableNameWithGuid) where T : class;
        Task<T> GetEntityInDataverse<T>(string table, string filter = null) where T : class;
        Task<T> PatchEntityInDataverse<T>(string table, T objectToPost) where T : class;
        Task<T> PostEntityInDataverse<T>(string table, T objectToPost) where T : class;
        Task<T> PutEntityInDataverse<T>(string table, T objectToPost) where T : class;
        Task<T> PatchEntityInDataverse<T>(string table, object objectToPost) where T : class;
        Task<T> PostEntityInDataverse<T>(string table, object objectToPost) where T : class;
        Task<T> PutEntityInDataverse<T>(string table, object objectToPost) where T : class;
    }
}