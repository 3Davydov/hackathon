using System.Globalization;
using hackathon.contracts;
using Microsoft.Extensions.Options;

namespace hackathon.model;
public class EmployeeLoaderOptions
{
    public string TeamLeadsFilePath { get; set; } = string.Empty;
    public string JuniorsFilePath { get; set; } = string.Empty;
}
public class EmployeeLoader
{
    private readonly EmployeeLoaderOptions _options;
    
    public EmployeeLoader(IOptions<EmployeeLoaderOptions> options)
    {
        _options = options.Value;
    }
    
    public List<Employee> LoadTeamLeads()
    {
        return LoadEmployees(_options.TeamLeadsFilePath);
    }

    public List<Employee> LoadJuniors()
    {
        return LoadEmployees(_options.JuniorsFilePath);
    }

    private List<Employee> LoadEmployees(string filePath)
    {
        var employees = new List<Employee>();
        var lines = File.ReadAllLines(filePath).Skip(1); /* Skip header */

        foreach (var line in lines)
        {
            var parts = line.Split(';');
            var id = int.Parse(parts[0], CultureInfo.InvariantCulture);
            var name = parts[1];
            employees.Add(new Employee(id, name));
        }

        return employees;
    }
}