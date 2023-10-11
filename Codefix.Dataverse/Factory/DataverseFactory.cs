using Codefix.Dataverse.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Codefix.Dataverse.Factory
{
    public class DataverseFactory : IDataverseFactory
    {
        private readonly DataverseAuthStore _configCollectorRepository;
        private readonly IServiceProvider _serviceProvider;

        public DataverseFactory(DataverseAuthStore configCollectorRepository, IServiceProvider serviceProvider)
        {
            _configCollectorRepository = configCollectorRepository;
            _serviceProvider = serviceProvider;
        }

        public IDataverseService CreateService(string KeyedService)
        {

            var config = _configCollectorRepository.GetDataVerseConfig(KeyedService) ?? throw new Exception("This key was not registered in the DataverseFactory");
            return new DataverseService(config, _serviceProvider.GetRequiredService<ILogger<DataverseService>>());
        }
    }
}
