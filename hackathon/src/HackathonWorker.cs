using System.Globalization;
using hackathon.contracts;
using hackathon.model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace hackathon;

public class HackathonWorker : IHostedService
{
    private readonly ILogger<HackathonWorker> _logger;
    private readonly Hackathon _hackathon;
    
    private readonly EmployeeLoader _employeeLoader;

    public HackathonWorker(ILogger<HackathonWorker> logger, EmployeeLoader employeeLoader, Hackathon hackathon)
    {
        _logger = logger;
        _employeeLoader = employeeLoader;
        _hackathon = hackathon;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hackathon worker started.");

        var teamLeads = _employeeLoader.LoadTeamLeads();
        var juniors = _employeeLoader.LoadJuniors();
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