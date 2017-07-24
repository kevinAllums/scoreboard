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

            FillGamesPanel();
        }
        
        private string CreateLink()
        {
            DateTime utcMinus10 = DateTime.UtcNow.AddHours(-10);

            string year = utcMinus10.Year.ToString();
            string month = "";
            if (utcMinus10.Month.ToString().Length < 2)
            {
                month = "0" + utcMinus10.Month.ToString();
            }
            else
            {
                month = utcMinus10.Month.ToString();
            }
            string day = "";
            if (utcMinus10.Day.ToString().Length < 2)
            {
                day = "0" + utcMinus10.Day.ToString();
            }
            else
            {
                day = utcMinus10.Day.ToString();
            }

            string link = string.Format("http://gd2.mlb.com/components/game/mlb/year_{0}/month_{1}/day_{2}/master_scoreboard.xml", 
                year, month, day);

            return link;
        }

        private void FillGamesPanel()
        {
            //XDocument doc = XDocument.Load("..\\..\\Games2.xml");
            string sourceXML = CreateLink();
            XDocument master_scoreboard = XDocument.Load(sourceXML);

            // Handle all in Progress games
            foreach (XElement game in master_scoreboard.Root.Descendants("game").
                Where(node => node.Element("status").Attribute("status").Value == "In Progress"))
            {
                Game gamePanel = new Game(game);

                GamesPanel.Children.Add(gamePanel);
            }

            // Handle all PreGame games
            foreach (XElement game in master_scoreboard.Root.Descendants("game").
                Where(node => node.Element("status").Attribute("status").Value == "Preview" ||
                node.Element("status").Attribute("status").Value == "Pre-Game" ||
                node.Element("status").Attribute("status").Value == "Warmup"))
            {
                Game gamePanel = new Game(game);

                GamesPanel.Children.Add(gamePanel);
            }

            // Handle PostGame games
            foreach (XElement game in master_scoreboard.Root.Descendants("game").
                Where(node => node.Element("status").Attribute("status").Value == "Game Over" ||
                node.Element("status").Attribute("status").Value == "Final"))
            {
                Game gamePanel = new Game(game);

                GamesPanel.Children.Add(gamePanel);
            }
        }
    }
}
