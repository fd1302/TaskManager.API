namespace FD.TaskManager.API.Extentions;

public static class ServiceExtentions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:7251")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });
}
