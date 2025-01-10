using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Communication;

public static class FileServiceExtensions
{

    public static IServiceCollection AddFileService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileServiceOptions>(configuration.GetSection(FileServiceOptions.FileService));
        
        services.AddHttpClient<FileHttpClient>((sp, config) =>
        {
            var fileOptions = sp.GetRequiredService<IOptions<FileServiceOptions>>().Value;
            
            config.BaseAddress = new Uri(fileOptions.Url);
        });
        
        return services;
    }
}