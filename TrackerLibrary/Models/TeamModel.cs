﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {

        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();


    }
}
