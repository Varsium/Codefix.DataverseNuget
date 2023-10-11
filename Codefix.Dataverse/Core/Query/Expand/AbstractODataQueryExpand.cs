using Codefix.Dataverse.Core.Extensions;
using Codefix.Dataverse.Core.Options;
using System.Text;

namespace Codefix.Dataverse.Core.Query.Expand
{
    internal abstract class AbstractODataQueryExpand
    {
        protected readonly ODataQueryBuilderOptions _odataQueryBuilderOptions;
        protected readonly StringBuilder _stringBuilder;

        public AbstractODataQueryExpand(StringBuilder stringBuilder, ODataQueryBuilderOptions odataQueryBuilderOptions)
        {
            _stringBuilder = stringBuilder;
            _odataQueryBuilderOptions = odataQueryBuilderOptions;
        }

        public StringBuilder Query => _stringBuilder.LastRemove(QuerySeparators.Nested);
    }
}
