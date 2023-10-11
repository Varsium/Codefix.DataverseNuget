using Codefix.Dataverse.Services;

namespace Codefix.Dataverse.Factory
{
    public interface IDataverseFactory
    {
        public IDataverseService CreateService(string KeyedService);
    }
}
