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
using System.Windows.Shapes;

using System.Xml.Linq;
using System.Data;

namespace scoreboard
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        private XElement game;
        public Details(XElement game)
        {
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            InitializeComponent();
            this.game = game;

            string title = string.Format("{0} at {1}",
                game.Attribute("away_name_abbrev").Value.ToString(),
                game.Attribute("home_name_abbrev").Value.ToString());

            this.Title = title;

            Console.WriteLine(game.Attribute("id").Value.ToString());
            CreateDetails();
        }

        private void CreateDetails()
        {
            string status = game.Element("status").Attribute("status").Value.ToString();

            if (status == "Preview" || status == "Pre-Game" || status == "Warmup")
            {
                CreatePreGameMatchup();
            }
            else if (status == "Final" || status == "Game Over" || status == "In Progress")
            {
                CreateLinescore();
                // eventually want to include boxscore
            }
            else if (status == "Delayed")
            {
                if (game.Element("linescore").Element("inning").Attribute("away") == null)
                {
                    CreatePreGameMatchup();
                }
                else
                {
                    CreateLinescore();
                }
            }
        }

        private void CreatePreGameMatchup()
        {
            AwayPitcherName.Content = string.Format("{0} {1}",
                    game.Element("away_probable_pitcher").Attribute("first_name").Value.ToString(),
                    game.Element("away_probable_pitcher").Attribute("last_name").Value.ToString());

            AwayPitcherPosNum.Content = string.Format("{0} #{1}",
                game.Element("away_probable_pitcher").Attribute("throwinghand").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("number").Value.ToString());

            AwayPitcherRecord.Content = string.Format("{0}-{1}, {2}",
                game.Element("away_probable_pitcher").Attribute("wins").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("losses").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("era").Value.ToString());

            HomePitcherName.Content = string.Format("{0} {1}",
                game.Element("home_probable_pitcher").Attribute("first_name").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("last_name").Value.ToString());

            HomePitcherPosNum.Content = string.Format("{0} #{1}",
                game.Element("home_probable_pitcher").Attribute("throwinghand").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("number").Value.ToString());

            HomePitcherRecord.Content = string.Format("{0}-{1}, {2}",
                game.Element("home_probable_pitcher").Attribute("wins").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("losses").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("era").Value.ToString());

            CityLabel.Content = game.Attribute("location").Value;

            PreGamePanel.Visibility = Visibility.Visible;
        }

        private void CreateLinescore()
        {
            DataTable DT = new DataTable();

            DataRow awayRow;
            DataRow homeRow;

            DataColumn DC = new DataColumn("Team");
            DT.Columns.Add(DC);

            awayRow = DT.NewRow();
            awayRow["Team"] = game.Attribute("away_name_abbrev").Value.ToString();
            homeRow = DT.NewRow();
            homeRow["Team"] = game.Attribute("home_name_abbrev").Value.ToString();

            int innings = 1;
            //string currentInning = game.Element("status").Attribute("inning").Value;
            //string currentHalf = game.Element("status").Attribute("inning_state").Value;
            foreach (XElement inning in game.Element("linescore").Elements().Where(node => node.Name == "inning"))
            {
                DataColumn column = new DataColumn(innings.ToString());
                DT.Columns.Add(column);

                //if (currentInning == inning.ToString())
                //{

                //}

                if (inning.Attribute("away") != null)
                {
                    awayRow[innings.ToString()] = inning.Attribute("away").Value.ToString();
                }
                if (inning.Attribute("home") != null)
                {
                    homeRow[innings.ToString()] = inning.Attribute("home").Value.ToString();
                }
                innings++;
            }
            // make the scoreboard at least 9 innings
            if (innings < 9)
            {
                for (int i = innings; i <= 9; i++)
                {
                    DataColumn column = new DataColumn(i.ToString());
                    DT.Columns.Add(column);
                }
            }

            DataColumn runColumn = new DataColumn("R");
            DT.Columns.Add(runColumn);
            awayRow["R"] = game.Element("linescore").Element("r").Attribute("away").Value.ToString();
            homeRow["R"] = game.Element("linescore").Element("r").Attribute("home").Value.ToString();

            DataColumn hitColumn = new DataColumn("H");
            DT.Columns.Add(hitColumn);
            awayRow["H"] = game.Element("linescore").Element("h").Attribute("away").Value.ToString();
            homeRow["H"] = game.Element("linescore").Element("h").Attribute("home").Value.ToString();

            DataColumn errorColumn = new DataColumn("E");
            DT.Columns.Add(errorColumn);
            awayRow["E"] = game.Element("linescore").Element("e").Attribute("away").Value.ToString();
            homeRow["E"] = game.Element("linescore").Element("e").Attribute("home").Value.ToString();

            DT.Rows.Add(awayRow);
            DT.Rows.Add(homeRow);

            LineScoreGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = DT });

            LinescorePanel.Visibility = Visibility.Visible;
        }
    }
}
