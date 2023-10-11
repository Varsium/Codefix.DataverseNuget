using Codefix.Dataverse.Core.Conventions.AddressingEntities;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources;
using Codefix.Dataverse.Core.Options;
using System.Linq.Expressions;
using System.Text;

namespace Codefix.Dataverse.Core.Builders
{
    public sealed class ODataQueryBuilder<TResource> : AbstractODataQueryBuilder
    {
        public ODataQueryBuilder(ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(odataQueryBuilderOptions)
        {
        }

        public ODataQueryBuilder(string baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(baseUrl, odataQueryBuilderOptions)
        {
        }

        public ODataQueryBuilder(Uri baseUrl, ODataQueryBuilderOptions odataQueryBuilderOptions = default)
            : base(baseUrl, odataQueryBuilderOptions)
        {
        }

        public IAddressingEntries<TEntity> For<TEntity>(Expression<Func<TResource, object>> resource) =>
           new ODataResource<TResource>(new StringBuilder(_baseUrl), _odataQueryBuilderOptions)
                .For<TEntity>(resource);
    }
}