using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration, Assembly entryAssembly)
    {
        Configuration = configuration;
        EntryAssembly = entryAssembly;
    }

    public IConfiguration Configuration { get; }
    public Assembly EntryAssembly { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<StoreContext>(options =>
        {
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddControllers()
            .AddApplicationPart(EntryAssembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/ping", async context => await context.Response.WriteAsync("pong"));
            endpoints.MapGet("/routes", async context =>
{
    var dataSource = context.RequestServices.GetRequiredService<Microsoft.AspNetCore.Routing.EndpointDataSource>();
    var names = dataSource.Endpoints.Select(e => e.DisplayName);
    await context.Response.WriteAsync(string.Join("\n", names));
});
        });

        
    }
}