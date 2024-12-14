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

        var teamLeads = LoadTeamLeads();
        var juniors = LoadJuniors();
        var teamLeadsWishlists = GenerateRandomWishlists(teamLeads, juniors);
        var juniorsWishlists = GenerateRandomWishlists(juniors, teamLeads);

        double result = _hackathon.RunHackathon(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);

        _logger.LogInformation($"Harmony: {result}");

        return Task.CompletedTask;
        
        /*  отделить загрузку людей, wish листов в отдельные классы + подробнее тесты */
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hackathon worker stopped.");
        return Task.CompletedTask;
    }

    public static List<Employee> LoadTeamLeads()
    {
        var teamLeads = new List<Employee>();
        var basePath = AppContext.BaseDirectory; // Получаем базовую директорию (bin/Debug/net8.0)
        var filePath = Path.Combine(basePath, "../../../resources/Teamleads20.csv");
        var lines = File.ReadAllLines(filePath).Skip(1); /* Skip header */

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            var id = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var name = parts[1];
            teamLeads.Add(new Employee(id, name));
        }

        return teamLeads;
    }

    public static List<Employee> LoadJuniors()
    {
        var juniors = new List<Employee>();
        var basePath = AppContext.BaseDirectory; // Base directory is (bin/Debug/net8.0)
        var filePath = Path.Combine(basePath, "../../../resources/Juniors20.csv");
        var lines = File.ReadAllLines(filePath).Skip(1); /* Skip header */

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            var id = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var name = parts[1];
            juniors.Add(new Employee(id, name));
        }

        return juniors;
    }

    public static List<WishList> GenerateRandomWishlists(List<Employee> employees, List<Employee> desiredEmployees)
    {
        var random = new Random();
        var desiredEmployeeIds = desiredEmployees.Select(e => e.Id).ToArray();
        return employees.Select(e => 
        {
            var shuffledDesiredEmployees = desiredEmployeeIds.OrderBy(_ => random.Next()).ToArray();
            return new WishList(e.Id, shuffledDesiredEmployees);
        }).ToList();
    }
}