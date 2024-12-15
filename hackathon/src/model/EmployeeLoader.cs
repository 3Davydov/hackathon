using System.Globalization;
using hackathon.contracts;

namespace hackathon.model;

public static class EmployeeLoader
{
    public static List<Employee> LoadTeamLeads()
    {
        var teamLeads = new List<Employee>();
        var basePath = AppContext.BaseDirectory; /* Get base directory (bin/Debug/net8.0) */
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
        var basePath = AppContext.BaseDirectory; /* Get base directory (bin/Debug/net8.0) */
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
}