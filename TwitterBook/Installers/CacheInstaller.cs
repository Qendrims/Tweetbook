using TwitterBook.Services;

namespace TwitterBook.Installers;
using TwitterBook.Cache;
public class CacheInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        var redisCacheSettings = new RedisCacheSettings();
        configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
        services.AddSingleton(redisCacheSettings);

        if (!redisCacheSettings.Enable)
        {
            return;
        }

        services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
    
}