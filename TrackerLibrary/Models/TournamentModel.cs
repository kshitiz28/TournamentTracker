using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {

        /// <summary>
        /// The unique identifier for the tournament.
        /// </summary>
        public int Id { get; set; }
        public string TournamentName { get; set; }

        public decimal EntryFee { get; set; }

        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        public List<MatchupModel> Prizes { get; set; } = new List<MatchupModel>();

        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

    }
}
