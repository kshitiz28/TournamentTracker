using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {

        IPrizeRequester callingForm;
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();

            callingForm= caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            if(ValidateForm())
            {
                MatchupModel model = new MatchupModel(
                    placeNameValue.Text,
                    placeNumberValue.Text,
                    prizeAmountValue.Text,
                    prizePercentageValue.Text);

                GlobalConfig.Connection.CreatePrize(model);

                callingForm.PrizeComplete(model);

                this.Close();

                //placeNameValue.Text = "";
                //placeNumberValue.Text = "";
                //prizeAmountValue.Text = "0";
                //prizePercentageValue.Text = "0";

            }
            else
            {
                MessageBox.Show("This form has invalid information. Please  Check it and try again.");
            }
        }

        private bool ValidateForm()
        {
            bool output = true;
            int placeNumber = 0;
            bool placeNumberValidNumber = int.TryParse(placeNumberValue.Text, out placeNumber);

            if(placeNumberValidNumber ==  false)
            {
                output= false;
            }

            if(placeNumber < 1) 
            {
                output= false;
            }

            if(placeNameValue.Text.Length == 0 )
            {
                output= false;
            }

            decimal prizeAmount = 0;
            double prizePercentage = 0;

            bool prizeAmountValid = decimal.TryParse(prizeAmountValue.Text, out prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageValue.Text, out prizePercentage);

            if (prizeAmountValid == false || prizePercentageValid == false) 
            {
                output= false;
            }

            if(prizeAmount <= 0 && prizePercentage <= 0) 
            {
                output= false;
            }

            if(prizePercentage < 0 || prizePercentage > 100) 
            {
                output= false;
            }

            return output;
        }
    }
}
