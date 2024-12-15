using hackathon.contracts;
using hackathon.model;
using hackathon.strategy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hackathon;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<HackathonWorker>();
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