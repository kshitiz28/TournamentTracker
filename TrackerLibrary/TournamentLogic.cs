using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order our list randomly of teams
        // Check if it is big enough - if not, add in byes - 2*2*2*2 - 2^4
        // Create our first round of matchups
        // Create every round after that - 8 matchups - 4 matchups - 2 matchups - 1 matchup

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);

            int rounds = FindNumbeOfRounds(randomizedTeams.Count);

            int byes = NumberOfByes(rounds,randomizedTeams.Count);
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;
            return output;
        }

        private static int FindNumbeOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while(val < teamCount)
            {
                // output = output +1;
                output += 1;

                //val= val *2;
                val *= 2;
            }

            return output;


        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams) 
        {
            return teams.OrderBy( x => Guid.NewGuid()).ToList();
        }
    }
}
