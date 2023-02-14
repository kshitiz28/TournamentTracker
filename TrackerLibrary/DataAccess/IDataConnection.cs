using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        MatchupModel CreatePrize(MatchupModel model);
        PersonModel CreatePerson(PersonModel model);

        TeamModel CreateTeam(TeamModel model);

        void CreateTournament(TournamentModel model);

        List<TeamModel> GetTeam_All();
        List<PersonModel> GetPerson_All();
    }
}
