using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {

        private TournamentModel tournament;
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();

            tournament= tournamentModel;

            LoadFormData();
        }

        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

        private void TournamentViewerForm_Load(object sender, EventArgs e)
        {

        }
    }
}