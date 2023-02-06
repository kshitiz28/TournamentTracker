using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PrizeModel
    {

        public int Id { get; set; }
        public int PlaceNumber { get; set; }

        public string PlaceName { get; set; }

        public decimal PrizeAmount { get; set; }

        public double PrizePercentage { get; set; }

        public PrizeModel() 
        {

        }

        public PrizeModel(string placeName,string placeNumber,string prizeAmount, string prizePercentage)
        {

            PlaceName = placeName;

            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount,out prizeAmountValue);
            PrizeAmount= prizeAmountValue;

            double prizePercentageValue = 0;

        }

    }
}
