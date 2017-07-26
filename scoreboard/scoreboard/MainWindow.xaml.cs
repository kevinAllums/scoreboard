using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Threading;

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
            this.MinWidth = 290 + SystemParameters.VerticalScrollBarWidth;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();

            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();

            FillGamesPanel();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            GamesPanel.Children.Clear();
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
            FillGamesPanel();
        }

        private string CreateLink()
        {
            // set it to this time so that it stays on the correct XML page until all games should be over
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

            // handle delayed games
            foreach (XElement game in master_scoreboard.Root.Descendants("game").
                Where(node => node.Element("status").Attribute("status").Value == "Delayed"))
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
