using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RhythmGameHelper.Shared;
using RhythmGameHelper.Admin;
using System;
using System.IO;
using System.Threading.Tasks;
using RhythmGameHelper.Admin.Model;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                string? environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                config.AddJsonFile($"appsettings.{environment ?? "Production"}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    options.UseNpgsql(connectionString);
                }
                );
                services.AddSingleton<Formatter>();
                services.AddSingleton<FSM>();
            });

        using var host = builder.Build();

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var fsm = services.GetRequiredService<FSM>();

                await fsm.Loop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
        }
    }
}