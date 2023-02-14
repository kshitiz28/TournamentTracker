using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackerLibrary.Models;


// * Load the text file
// * Convert the text to List<PrizeModel>
// Find the max ID
// Add the new record with the new ID (max+1)
// Convert the prizes to List<string>
// Save the List<string> to the text file

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName) //PrizeModels.csv
        {
            // D:\Learning Start\data\TournamentTracker\PrizeModels.csv
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{ fileName }";
        }

        public static List<string> LoadFile(this string file) 
        {
            if(!File.Exists(file)) 
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<MatchupModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<MatchupModel> output = new List<MatchupModel>();

            foreach(string line in lines) 
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount= decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);

            }

            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach(string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id= int.Parse(cols[0]);
                p.FirstName= cols[1];
                p.LastName= cols[2];
                p.EmailAddress= cols[3];
                p.CellPhoneNumber= cols[4];

                output.Add(p);
            }

            return output;
        }


        public static List<TeamModel> ConvertToTeamModels(this List<string> lines , string peopleFileName)
        {
            //id,team name,list of ids seperated by the pipe
            //3,Tim's Team,1|3|5

            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();


            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                TeamModel t = new TeamModel();
                t.Id= int.Parse(cols[0]);
                t.TeamName= cols[1];

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds) 
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);

            }

            return output;

        }
        public static void SaveToPrizeFile(this List<MatchupModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(MatchupModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellPhoneNumber }");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if(people.Count == 0 )
            {
                return "";
            }

            foreach(PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            output = output.Substring(0, output.Length - 1); //take substring without the last '|'
            
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines,
            string teamFileName,
            string peopleFileName,
            string prizeFileName)
        {
            // id = 0
            // TournamentName = 1
            // EntryFee =2
            // Enteredteams = 3
            // Prizes = 4
            // Rounds = 5
            // id,TournamentName,EntryFee,(id|id|id -Entered Teams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)
            List<TournamentModel> output = new List<TournamentModel>();

            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);

            List<MatchupModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach(string line in lines) 
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');

                foreach(string id in teamIds) 
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                string[] prizeIds = cols[4].Split('|');

                foreach (string id in prizeIds)
                {
                    tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                }

                // TODO - Capture Rounds Information

                output.Add(tm);
            }

            return output;
        }

        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(TournamentModel tm in models) 
            {
                lines.Add($@"{ tm.Id },
                    { tm.TournamentName },
                    { tm.EntryFee },
                    { ConvertTeamListToString(tm.EnteredTeams) },
                    { ConvertPrizeListToString(tm.Prizes) },
                    { ConvertRoundListToString(tm.Rounds) }");
            }
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ ConvertMatchupListToString(r) }|";
            }

            output = output.Substring(0, output.Length - 1); //take substring without the last '|'

            return output;

        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel m in matchups)
            {
                output += $"{ m.Id }^";
            }

            output = output.Substring(0, output.Length - 1); //take substring without the last '|'

            return output;
        }

        private static string ConvertPrizeListToString(List<MatchupModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel p in prizes)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1); //take substring without the last '|'

            return output;

        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            output = output.Substring(0, output.Length - 1); //take substring without the last '|'

            return output;

        }

        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            // Loop through each Round
            // Loop through each Matchup
            // Get the id for the new matchup and save the record
            // Loop through each Entry, get the id and save it.

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    // Load all of the matchups from file
                    // Get the top id and add one
                    // Store the id
                    // Save the matchup record

                    matchup.SaveMatchupToFile(matchupFile,matchupEntryFile);

                }
            }

        }

        public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {

            // id = 0, TeamCompeting =1, Score=2, ParentMatchup =3
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupEntryModel me = new MatchupEntryModel();
                me.Id = int.Parse(cols[0]);
                me.TeamCompeting= LookupTeamById(int.Parse(cols[1]));
                me.Score = double.Parse(cols[2]);

                int parentId = 0;
                if (int.TryParse(cols[3],out parentId))
                {
                    me.ParentMatchup = LookupMatchupById(int.Parse(cols[3]));
                }
                else
                {
                    me.ParentMatchup = null;
                }
                
                output.Add(me);
            }

            return output;
        }

        private static List<MatchupEntryModel> ConvertStringToMatchupEntryModel(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();

            foreach (string id in ids)
            {
                output.Add(entries.Where(x => x.Id ==  int.Parse(id)).First());
            }

            return output;
        }


        private static TeamModel LookupTeamById(int id)
        {
            List<TeamModel> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(GlobalConfig.PeopleFile);

            return teams.Where(x => x.Id == id).First();

        }

        private static MatchupModel LookupMatchupById(int id)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();

            return matchups.Where(x => x.Id == id).First();
        }
        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {

            // id =0, entries = 1(pipe delimited by id),winner =2 ,matchupRound =3 
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]);
                p.Entries = ConvertStringToMatchupEntryModel(cols[1]);
                p.Winner = LookupTeamById(int.Parse(cols[2]));
                p.MatchupRound = int.Parse(cols[3]);

                output.Add(p);

            }

            return output;
        }

        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile,string matchupEntryFile)
        {
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }
        }

        public static void SaveEntryToFile(this MatchupEntryModel , string matchupEntryFile)
        {

        }
    }
}
