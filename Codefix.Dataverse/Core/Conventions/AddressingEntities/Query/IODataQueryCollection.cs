using Codefix.Dataverse.Core.Conventions.AddressingEntities.Options;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Query
{
    public interface IODataQueryCollection<TEntity> : IODataOptionCollection<IODataQueryCollection<TEntity>, TEntity>
    {
        public Task<IList<TEntity>> ToListAsync();
        public string ToOdataQuery();
        public Task<TEntity> FirstOrDefaultAsync();

        public Task<TEntity> CreateAsync(TEntity entity);

        public Task<bool> DeleteAsync(Guid id);

        public Task<TEntity> UpdateAsync(TEntity entity);


        public Task<TEntity> UpdateAsync(Guid id, TEntity entity);
        public Task<TEntity> FirstOrDefaultAsync(Guid id);
    }
}