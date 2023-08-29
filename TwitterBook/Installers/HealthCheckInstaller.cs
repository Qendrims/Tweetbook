namespace TwitterBook.Installers;

public class HealthCheckInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddHealthChecks();
        
    }
    
}