using Xunit;

namespace hackathon.tests.tests;

public class WishListGenerationTest
{
    [Fact]
    public void Wishlist_Size_ShouldBeEqualToParticipants()
    {
        var juniors = HackathonWorker.LoadJuniors();
        var teamLeads = HackathonWorker.LoadTeamLeads();

        var wishlists = HackathonWorker.GenerateRandomWishlists(juniors, teamLeads);
        Assert.Equal(20, wishlists.Count); // Should match participant count
    }

    [Fact]
    public void SpecificEmployee_ShouldExistInWishlist()
    {
        var juniors = HackathonWorker.LoadJuniors();
        var teamLeads = HackathonWorker.LoadTeamLeads();

        var wishlists = HackathonWorker.GenerateRandomWishlists(juniors, teamLeads);
        var specificJuniorId = juniors.First().Id;
        
        // FluentAssertation.AreEqual(specificJuniorId, juniors.First().Id);
        Assert.Contains(wishlists, w => w.DesiredEmployees.Contains(specificJuniorId));
    }
}