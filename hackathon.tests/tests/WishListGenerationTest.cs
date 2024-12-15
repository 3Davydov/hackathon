using hackathon.model;
using Xunit;

namespace hackathon.tests.tests;

public class WishListGenerationTest
{
    [Fact]
    public void Wishlist_Size_ShouldBeEqualToParticipants()
    {
        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);
        Assert.Equal(20, wishlists.Count); /* Should match participant count */
        Assert.All(wishlists, w => Assert.NotNull(w)); /* Ensure no null wishlists exist */
    }
    
    [Fact]
    public void AllJuniors_ShouldBeAssignedToAWishlist()
    {
        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var junior in juniors)
        {
            Assert.Contains(wishlists, w => w.DesiredEmployees.Contains(junior.Id));
        }
    }
    
    [Fact]
    public void AllTeamLeads_ShouldBeAssignedToAWishlist()
    {
        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var teamLead in teamLeads)
        {
            Assert.Contains(wishlists, w => w.EmployeeId == teamLead.Id);
        }
    }
    
    [Fact]
    public void NoWishlist_ShouldNotHaveNullValues()
    {
        var juniors = EmployeeLoader.LoadJuniors();
        var teamLeads = EmployeeLoader.LoadTeamLeads();

        var wishlists = WishListGenerator.GenerateRandomWishlists(juniors, teamLeads);

        foreach (var wishlist in wishlists)
        {
            Assert.NotNull(wishlist);
        }
    }
}