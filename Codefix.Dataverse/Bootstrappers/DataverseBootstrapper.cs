using Codefix.Dataverse.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace Codefix.Dataverse.Bootstrappers
{
    public static class DataverseBootstrapper
    {
        /// <summary>
        /// This is for Creating several instances with the DataverseFactory. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configCollectorService"></param>
        /// <exception cref="Exception"></exception>
        /// 
        public static IServiceCollection AddDataverseFactory(this IServiceCollection services, Action<DataverseAuthStoreOptions> configOptions)
        {
            services.AddLogging();
            var option = new DataverseAuthStoreOptions();
            configOptions?.Invoke(option);
            services.AddSingleton((sp) => option);
            services.AddSingleton<DataverseAuthStore>();
            services.AddSingleton<IDataverseFactory, DataverseFactory>();
            return services;
        }
    }
}
