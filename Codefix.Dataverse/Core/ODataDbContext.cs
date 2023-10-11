using Codefix.Dataverse.Core.Options;
using Codefix.Dataverse.Services;
using System.Text;

namespace Codefix.Dataverse.Core
{
    public abstract class ODataDbContext
    {
        private IDataverseService _dataverseService;
        private Type _parentClass;

        protected ODataDbContext()
        {

        }

        internal void InitDbSets()
        {
            var props = _parentClass.GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType.Name.Contains(nameof(ODataDbSet<object>)) &&
                    !props[i].PropertyType.IsInterface)
                {
                    var instance = Activator.CreateInstance(props[i].PropertyType, new StringBuilder(),
                        new ODataQueryBuilderOptions(), _dataverseService);
                    props[i].SetValue(this, instance);
                }
            }
        }

        internal void SetDataverseProvider(IDataverseService namedProvider, Type parentClass)
        {
            _dataverseService = namedProvider;
            _parentClass = parentClass;
            InitDbSets();
        }
    }
}