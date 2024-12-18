using hackathon.contracts;
using hackathon.model;
using hackathon.strategy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hackathon;

public class Program
{
    public static IConfiguration Configuration { get; private set; }
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.Configure<EmployeeLoaderOptions>(context.Configuration.GetSection("EmployeeLoader"));
                services.AddHostedService<HackathonWorker>();
                services.AddSingleton<EmployeeLoader>();
                services.AddTransient<HRManager>();
                services.AddTransient<HRDirector>();
                services.AddTransient<Hackathon>();
                services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
                services.AddTransient<ITeamBuildingStrategy, TeamLeadsHateTheirJuniorsStrategy>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .Build();

        host.Run();
    }
}