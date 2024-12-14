using hackathon.contracts;

namespace hackathon.model;

public class Hackathon
{
    private readonly HRManager _hrManager;
    private readonly HRDirector _hrDirector;

    public Hackathon(HRManager hrManager, HRDirector hrDirector)
    {
        _hrManager = hrManager;
        _hrDirector = hrDirector;
    }

    public double RunHackathon(List<Employee> teamLeads, List<Employee> juniors, List<WishList> teamLeadsWishlists,
                               List<WishList> juniorsWishlists)
    {
        var teams = _hrManager.CreateTeams(teamLeads, juniors, teamLeadsWishlists, juniorsWishlists);
        return _hrDirector.CalculateHarmony(teams, teamLeadsWishlists, juniorsWishlists);
    }
}