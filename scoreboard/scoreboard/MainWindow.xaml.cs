using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Xml.Linq;
using System.Net.Http;

namespace scoreboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.MaxHeight = (SystemParameters.MaximizedPrimaryScreenHeight / 8) * 7;
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;
            InitializeComponent();

            GetXML();
        }

        private async void GetXML()
        {
            HttpClient client = new HttpClient();
            string xml = CreateLink();
            HttpResponseMessage response = await client.GetAsync(xml);

            if (response.IsSuccessStatusCode)
            {
                CreateGames(xml);
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }

        private string CreateLink()
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            string year = easternTime.Year.ToString();
            string month = "";
            if (easternTime.Month.ToString().Length < 2)
            {
                month = "0" + easternTime.Month.ToString();
            }
            else
            {
                month = easternTime.Month.ToString();
            }
            string day = "";
            if (easternTime.Day.ToString().Length < 2)
            {
                day = "0" + easternTime.Day.ToString();
            }
            else
            {
                day = easternTime.Day.ToString();
            }

            string link = string.Format("http://gd2.mlb.com/components/game/mlb/year_{0}/month_{1}/day_{2}/scoreboard.xml", year, month, day);

            return link;
        }

        private void CreateGames(string xml)
        {
            //XDocument doc = XDocument.Load("..\\..\\Games2.xml");
            XDocument doc = XDocument.Load(xml);

            int numInProgressGames = CreateInProgressGames(doc);
            int numPreGames = CreatePreGameGames(doc);
            int numGamesOver = CreateOverGames(doc);

            Console.WriteLine("Games in Progress: " + numInProgressGames);
            Console.WriteLine("Games in Pregame: " + numPreGames);
            Console.WriteLine("Games Over: " + numGamesOver);
        }

        private int CreateInProgressGames(XDocument doc)
        {
            return 0;
        }

        private int CreatePreGameGames(XDocument doc)
        {
            return 0;
        }

        private int CreateOverGames(XDocument doc)
        {
            return 0;
        }
    }
}
