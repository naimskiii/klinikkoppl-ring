// Startup.cs

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Infrastructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
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

        // Stram inn CORS til kun de adressene som faktisk skal få snakke med API-et.
        // Legg til flere localhost-porter her etter behov mens du utvikler.
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "https://klinikkopplaering.no",
                        "http://localhost:5500",
                        "http://localhost:8000"
                      )
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Maks 5 innloggingsforsøk per minutt per klient, resten avvises automatisk.
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("LoginPolicy", opt =>
            {
                opt.PermitLimit = 5;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 0;
            });
            options.RejectionStatusCode = 429;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Tving HTTPS kun utenfor lokal utvikling, slik at localhost-testing
            // med vanlig http fortsatt fungerer som i dag.
            app.UseHttpsRedirection();
        }

        app.UseRouting();

        app.UseCors("AllowFrontend");
        app.UseRateLimiter();

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