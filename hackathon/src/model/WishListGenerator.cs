using hackathon.contracts;

namespace hackathon.model;

public static class WishListGenerator
{
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
    
    public static List<WishList> GenerateStaticWishlists(List<Employee> employees, List<Employee> desiredEmployees)
    {
        var desiredEmployeeIds = desiredEmployees.Select(e => e.Id).OrderBy(id => id).ToArray();
        return employees.Select(e => new WishList(e.Id, desiredEmployeeIds)).ToList();
    }
}