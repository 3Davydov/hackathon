using System.Globalization;
using hackathon.contracts;
using hackathon.model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hackathon;

public class HackathonWorker : IHostedService
{
    private readonly ILogger<HackathonWorker> _logger;
    private readonly HRManager _hrManager;
    private readonly HRDirector _hrDirector;
    private readonly Hackathon _hackathon;

    public HackathonWorker(ILogger<HackathonWorker> logger, HRManager hrManager, HRDirector hrDirector, Hackathon hackathon)
    {
        _logger = logger;
        _hrManager = hrManager;
        _hrDirector = hrDirector;
        _hackathon = hackathon;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hackathon worker started.");

        var teamLeads = EmployeeLoader.LoadTeamLeads();
        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeadsWishLists = WishListGenerator.GenerateRandomWishlists(teamLeads, juniors);
        var juniorsWishLists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        var result = _hackathon.RunHackathon(teamLeads, juniors, teamLeadsWishLists, juniorsWishLists);

        _logger.LogInformation($"Harmony: {result}");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hackathon worker stopped.");
        return Task.CompletedTask;
    }
}