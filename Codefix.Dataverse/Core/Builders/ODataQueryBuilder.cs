using Codefix.Dataverse.Core.Conventions.AddressingEntities;
using Codefix.Dataverse.Core.Conventions.AddressingEntities.Resources;
using Codefix.Dataverse.Core.Options;
using System.Text;

namespace Codefix.Dataverse.Core.Builders
{
    public sealed class ODataQueryBuilder : AbstractODataQueryBuilder
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

        public IAddressingEntries<TEntity> For<TEntity>(string resource) =>
           new ODataResource(new StringBuilder(_baseUrl), _odataQueryBuilderOptions)
                .For<TEntity>(resource);
    }
}
