using hackathon.contracts;
using Xunit;

namespace hackathon.model;

public class HRDirector
{
    public double CalculateHarmony(List<Team> teams, List<WishList> teamLeadsWishlists, List<WishList> juniorsWishlists)
    {
        var sumOfReciprocals = 0.0;
        var totalParticipants = teams.Count * 2; /* Each team has team lead and junior */

        foreach (var team in teams)
        {
            var teamLeadWishlist = teamLeadsWishlists.FirstOrDefault(w => w.EmployeeId == team.TeamLead.Id);
            Assert.NotNull(teamLeadWishlist);
            
            var teamLeadSatisfaction = GetSatisfactionIndex(teamLeadWishlist, team.Junior.Id);

            var juniorWishlist = juniorsWishlists.FirstOrDefault(w => w.EmployeeId == team.Junior.Id);
            Assert.NotNull(juniorWishlist);
            var juniorSatisfaction = GetSatisfactionIndex(juniorWishlist, team.TeamLead.Id);

            /* Add the reciprocals of satisfaction scores for both team lead and junior */
            if (teamLeadSatisfaction > 0)
                sumOfReciprocals += 1.0 / teamLeadSatisfaction;

            if (juniorSatisfaction > 0)
                sumOfReciprocals += 1.0 / juniorSatisfaction;
        }

        /* Harmonic mean formula: n / sum(1 / x_i) */
        return totalParticipants / sumOfReciprocals;
    }

    private int GetSatisfactionIndex(WishList wishlist, int employeeId)
    {
        var index = Array.IndexOf(wishlist.DesiredEmployees, employeeId);
        if (index < 0) return 0;
        
        /* The closer to the top of the list, the greater the weight */
        var weight = wishlist.DesiredEmployees.Length - index;
        return weight;
    }
}