using Azure.Core;
using Codefix.Dataverse.Authentication;
using Codefix.Dataverse.Configs;
using Codefix.Dataverse.Core;
using Codefix.Dataverse.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Codefix.Dataverse.Bootstrappers
{
    public static class ServiceBootstrapper
    {
        public static IServiceCollection AddOdataDbContext<TSource>(this IServiceCollection services, string baseUrl, string tenantId, string clientId, string clientSecret) where TSource : ODataDbContext
        {
            services.AddLogging();
            services.AddSingleton<IDataverseService, DataverseService>((sp) =>
            { return new DataverseService(new DataverseAuthConfig(baseUrl, tenantId, clientId, clientSecret), sp.GetRequiredService<ILogger<DataverseService>>()); });

            services.AddTransient((sp) =>
            {
                var dc = sp.GetRequiredService<IDataverseService>();
                var instance = Activator.CreateInstance(typeof(TSource)) as TSource;
                instance.SetDataverseProvider(dc, typeof(TSource));
                return instance;
            });
            return services;
        }
        public static IServiceCollection AddOdataDbContext<TSource>(this IServiceCollection services, string baseUrl, TokenCredential credential) where TSource : ODataDbContext
        {
            services.AddLogging();
            services.AddSingleton<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(baseUrl, credential), sp.GetRequiredService<ILogger<DataverseService>>()));

            services.AddTransient((sp) =>
            {
                var dc = sp.GetRequiredService<IDataverseService>();
                var instance = Activator.CreateInstance(typeof(TSource)) as TSource;
                instance.SetDataverseProvider(dc, typeof(TSource));
                return instance;
            });
            return services;

        }
        public static IServiceCollection AddOdataDbContext<TSource>(this IServiceCollection services, Action<OdataDbContextOptions> options) where TSource : ODataDbContext
        {
            services.AddLogging();
            var dataDbContextOptions = new OdataDbContextOptions();
            options?.Invoke(dataDbContextOptions);

            if (dataDbContextOptions.WithManagedIdentity())
            {
                services.AddSingleton<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(dataDbContextOptions.BaseUrl, dataDbContextOptions.Credentials), sp.GetRequiredService<ILogger<DataverseService>>()));

            }
            else
            {
                services.AddSingleton<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(dataDbContextOptions.BaseUrl, dataDbContextOptions.TenantId, dataDbContextOptions.ClientId, dataDbContextOptions.ClientSecret), sp.GetRequiredService<ILogger<DataverseService>>()));

            }
            services.AddTransient((sp) =>
            {
                var dc = sp.GetRequiredService<IDataverseService>();
                var instance = Activator.CreateInstance(typeof(TSource)) as TSource;
                instance.SetDataverseProvider(dc, typeof(TSource));
                return instance;
            });
            return services;
        }

        public static IServiceCollection AddDataverse(this IServiceCollection services, Action<DataverseServiceOptions> options)
        {
            services.AddLogging();
            var dataverseServiceOptions = new DataverseServiceOptions();
            options?.Invoke(dataverseServiceOptions);
            if ((dataverseServiceOptions.Config?.WithInterActiveCredentials()).GetValueOrDefault(false))
            {
                switch (dataverseServiceOptions.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddDataverseAsSingleton(dataverseServiceOptions.Config!);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddDataverseAsScoped(dataverseServiceOptions.Config!);
                        break;
                    default:
                        services.AddDataverseAsTransient(dataverseServiceOptions.Config!);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(dataverseServiceOptions.BaseUrl) && dataverseServiceOptions.Credential is not null)
            {
                switch (dataverseServiceOptions.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddDataverseAsSingleton(dataverseServiceOptions.BaseUrl, dataverseServiceOptions.Credential);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddDataverseAsScoped(dataverseServiceOptions.BaseUrl, dataverseServiceOptions.Credential);
                        break;
                    default:
                        services.AddDataverseAsTransient(dataverseServiceOptions.BaseUrl, dataverseServiceOptions.Credential);
                        break;
                }
            }
            return services;
        }
        public static IServiceCollection AddDataverseAsSingleton(this IServiceCollection services, string baseUrl, TokenCredential credential)
        {
            services.AddLogging();
            services.AddSingleton<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(baseUrl, credential), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;

        }
        public static IServiceCollection AddDataverseAsScoped(this IServiceCollection services, string baseUrl, TokenCredential credential)
        {
            services.AddLogging();
            services.AddScoped<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(baseUrl, credential), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;
        }
        public static IServiceCollection AddDataverseAsTransient(this IServiceCollection services, string baseUrl, TokenCredential credential)
        {
            services.AddLogging();
            services.AddScoped<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(baseUrl, credential), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;
        }

        public static IServiceCollection AddDataverseAsSingleton(this IServiceCollection services, DataverseConfig config)
        {
            services.AddLogging();
            services.AddSingleton<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(config), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;

        }
        public static IServiceCollection AddDataverseAsScoped(this IServiceCollection services, DataverseConfig config)
        {
            services.AddLogging();
            services.AddScoped<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(config), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;
        }
        public static IServiceCollection AddDataverseAsTransient(this IServiceCollection services, DataverseConfig credential)
        {
            services.AddLogging();
            services.AddScoped<IDataverseService, DataverseService>((sp) => new DataverseService(new DataverseAuthConfig(credential), sp.GetRequiredService<ILogger<DataverseService>>()));
            return services;
        }

    }
}
