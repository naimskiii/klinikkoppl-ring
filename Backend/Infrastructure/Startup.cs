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

        // CORS: kun disse adressene får lov til å kalle API-et fra nettleseren.
        // Merk: klinikkopplæring.no bruker "æ", som nettlesere omgjør til
        // punycode (xn--klinikkopplring-7lb.no) i Origin-headeren — begge
        // former må derfor stå på lista.
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "https://klinikkopplaering.no",
                        "https://www.klinikkopplaering.no",
                        "https://xn--klinikkopplring-7lb.no",
                        "https://www.xn--klinikkopplring-7lb.no",
                        "http://localhost:5500",
                        "http://localhost:8000"
                      )
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        // Maks 5 innloggingsforsøk per minutt per klient.
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