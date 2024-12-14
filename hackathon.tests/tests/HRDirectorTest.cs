using hackathon.contracts;
using hackathon.model;

namespace hackathon.tests.tests;

using Xunit;
public class HrDirectorTest
{
    [Fact]
    public void CalculateHarmony_ShouldReturnExpectedResult()
    {
        var director = new HRDirector();
        var teams = new List<Team>
        {
            new Team(new Employee(1, "TL1"), new Employee(1, "J1")),
            new Team(new Employee(2, "TL2"), new Employee(2, "J2"))
        };

        var teamLeadsWishlists = new List<WishList>
        {
            new WishList(1, [1, 2]),
            new WishList(2, [2, 1])
        };
        var juniorsWishlists = new List<WishList>
        {
            new WishList(1, [1, 2]),
            new WishList(2, [2, 1])
        };

        var harmony = director.CalculateHarmony(teams, teamLeadsWishlists, juniorsWishlists);
        Assert.Equal(2, harmony);
    }
}