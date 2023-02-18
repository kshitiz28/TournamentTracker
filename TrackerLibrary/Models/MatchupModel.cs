using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// The Unique Identifier for the matchup.
        /// </summary>
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();


        /// <summary>
        /// The ID from the database that will be used to identify the winner.
        /// </summary>
        public int WinnerID { get; set; }

        public TeamModel Winner { get; set; }

        public int MatchupRound { get; set; }
    }
}