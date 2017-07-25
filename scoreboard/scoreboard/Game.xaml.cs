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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : UserControl
    {
        private XElement game;
        public Game(XElement game)
        {
            this.game = game;
            InitializeComponent();
            CreateGame();
        }

        private void CreateGame()
        {
            string gameStatus = game.Element("status").Attribute("status").Value;
            // in progress games
            if (gameStatus == "In Progress")
            {
                CreateGameInProgress();
            }
            // pre games
            else if (gameStatus == "Preview" || gameStatus == "Pre-Game" || gameStatus == "Warmup")
            {
                CreatePreGame();
            }
            // post games
            else if (gameStatus == "Game Over" || gameStatus == "Final")
            {
                CreatePostGame();
            }
            // delayed games
            else
            {
                CreateDelayedGame();
            }
        }

        private void CreateGameInProgress()
        {
            SetNamesAndRecords();
            //score
            SetScore();
            // game information
            GameInProgressPanel.Visibility = Visibility.Visible;
            // inning & half
            inningLabel.Content = string.Format("{0} {1}",
                game.Element("status").Attribute("inning_state").Value.ToString(),
                game.Element("status").Attribute("inning").Value.ToString());
            // count and outs
            CountAndOutsLabel.Content = string.Format("{0}-{1}, {2} out",
                game.Element("status").Attribute("b").Value.ToString(),
                game.Element("status").Attribute("s").Value.ToString(),
                game.Element("status").Attribute("o").Value.ToString());
            // base runners
            int base_runner_status = Convert.ToInt32(game.Element("runners_on_base").Attribute("status").Value);
            SolidColorBrush brush = new SolidColorBrush(System.Windows.Media.Colors.Orange);

            if (base_runner_status == 0)
            {
                // No one on base
            }
            else if (base_runner_status == 1) // correct
            {
                // 1st
                FirstBase.Fill = brush;
            }
            else if (base_runner_status == 2) // correct
            {
                // 2nd
                SecondBase.Fill = brush;
            }
            else if (base_runner_status == 3) // correct
            {
                // 3rd
                ThirdBase.Fill = brush;
            }
            else if (base_runner_status == 4) // correct
            {
                // 1st/2nd
                FirstBase.Fill = brush;
                SecondBase.Fill = brush;
            }
            else if (base_runner_status == 5)
            {
                // 1st/3rd
                FirstBase.Fill = brush;
                ThirdBase.Fill = brush;
            }
            else if (base_runner_status == 6) // correct
            {
                // 2nd/3rd
                SecondBase.Fill = brush;
                ThirdBase.Fill = brush;
            }
            else if (base_runner_status == 7) // correct
            {
                // 2nd/3rd
                FirstBase.Fill = brush;
                SecondBase.Fill = brush;
                ThirdBase.Fill = brush;
            }
            else if (base_runner_status == 8)
            {
                // 1st/2nd/3rd
                FirstBase.Fill = brush;
                SecondBase.Fill = brush;
                ThirdBase.Fill = brush;
            }
            // current pitcher
            FirstRowLabel.Content = string.Format("P: {0} ({1}-{2}, {3})",
                game.Element("pitcher").Attribute("name_display_roster").Value.ToString(),
                game.Element("pitcher").Attribute("wins").Value.ToString(),
                game.Element("pitcher").Attribute("losses").Value.ToString(),
                game.Element("pitcher").Attribute("era").Value.ToString());
            // current batter
            SecondRowLabel.Content = string.Format("AB: {0} ({1}-{2}, {3})",
                game.Element("batter").Attribute("name_display_roster").Value.ToString(),
                game.Element("batter").Attribute("h").Value.ToString(),
                game.Element("batter").Attribute("ab").Value.ToString(),
                game.Element("batter").Attribute("avg").Value.ToString());

            ThirdRowLabel.Content = "";
        }

        private void CreatePreGame()
        {
            SetNamesAndRecords();
            SetScore();
            // game information
            if (game.Element("status").Attribute("status").Value == "Preview" || game.Element("status").Attribute("status").Value == "Pre-Game")
            {
                PreOrPostGameLabel.Content = string.Format("{0}{1} {2}",
                    game.Attribute("time").Value.ToString(),
                    game.Attribute("ampm").Value.ToString(),
                    game.Attribute("time_zone").Value.ToString());

                PreOrPostGameLabel.Visibility = Visibility.Visible;
            }
            else if (game.Element("status").Attribute("status").Value == "Warmup")
            {
                PreOrPostGameLabel.Content = string.Format("{0}{1} {2}\nWarmup",
                    game.Attribute("time").Value.ToString(),
                    game.Attribute("ampm").Value.ToString(),
                    game.Attribute("time_zone").Value.ToString());

                PreOrPostGameLabel.Visibility = Visibility.Visible;
            }
            else
            {
                PreOrPostGameLabel.Visibility = Visibility.Collapsed;
            }
            // away probable pitcher
            FirstRowLabel.Content = string.Format("{0}: {1} ({2}-{3}, {4})",
                game.Attribute("away_name_abbrev").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("name_display_roster").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("wins").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("losses").Value.ToString(),
                game.Element("away_probable_pitcher").Attribute("era").Value.ToString());
            // home probable pitcher
            SecondRowLabel.Content = string.Format("{0}: {1} ({2}-{3}, {4})",
                game.Attribute("home_name_abbrev").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("name_display_roster").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("wins").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("losses").Value.ToString(),
                game.Element("home_probable_pitcher").Attribute("era").Value.ToString());
            
            ThirdRowLabel.Content = "";
        }

        private void CreateDelayedGame()
        {
            // if game delayed while in progress
            if (game.Element("linescore").Element("inning").Attribute("away") != null)
            {
                CreateGameInProgress();
                ThirdRowLabel.Content = game.Element("status").Attribute("reason").Value.ToUpper();
                ThirdRowLabel.Content += " " + game.Element("status").Attribute("status").Value.ToUpper();
            }
            // if game is delayed before game start
            else
            {
                CreatePreGame();
                PreOrPostGameLabel.Content += game.Element("status").Attribute("reason").Value.ToUpper();
                PreOrPostGameLabel.Content += " " + game.Element("status").Attribute("status").Value.ToUpper();
            }
        }

        private void CreatePostGame()
        {
            SetNamesAndRecords();
            // score
            SetScore();
            // game state
            PreOrPostGameLabel.Content = "Final";
            PreOrPostGameLabel.Visibility = Visibility.Visible;

            // winning pitcher
            FirstRowLabel.Content = string.Format("W: {0} ({1}-{2}, {3})",
                game.Element("winning_pitcher").Attribute("name_display_roster").Value.ToString(),
                game.Element("winning_pitcher").Attribute("wins").Value.ToString(),
                game.Element("winning_pitcher").Attribute("losses").Value.ToString(),
                game.Element("winning_pitcher").Attribute("era").Value.ToString());
            // losing pitcher
            SecondRowLabel.Content = string.Format("L: {0} ({1}-{2}, {3})",
                game.Element("losing_pitcher").Attribute("name_display_roster").Value.ToString(),
                game.Element("losing_pitcher").Attribute("wins").Value.ToString(),
                game.Element("losing_pitcher").Attribute("losses").Value.ToString(),
                game.Element("losing_pitcher").Attribute("era").Value.ToString());
            // save pitcher
            if (game.Element("save_pitcher").Attribute("id").Value.ToString() != "")
            {
                ThirdRowLabel.Content = string.Format("S: {0} ({1})",
                    game.Element("save_pitcher").Attribute("name_display_roster").Value.ToString(),
                    game.Element("save_pitcher").Attribute("saves").Value.ToString());
            }
            else
            {
                ThirdRowLabel.Content = "";
            }
        }

        private void SetNamesAndRecords()
        {
            // away team name and record
            AwayTeamLabel.Content = game.Attribute("away_team_name").Value.ToString();
            string awayRecord = string.Format("({0}-{1})",
                game.Attribute("away_win").Value.ToString(),
                game.Attribute("away_loss").Value.ToString());
            AwayTeamRecordLabel.Content = awayRecord;
            // home team name and record
            HomeTeamLabel.Content = game.Attribute("home_team_name").Value.ToString();
            string homeRecord = string.Format("({0}-{1})",
                game.Attribute("home_win").Value.ToString(),
                game.Attribute("home_loss").Value.ToString());
            HomeTeamRecordLabel.Content = homeRecord;
        }

        private void SetScore()
        {
            string status = game.Element("status").Attribute("status").Value;
            if (status == "Preview" || status == "Pre-Game" || status == "Warmup")
            {
                AwayTeamScoreLabel.Visibility = Visibility.Hidden;
                HomeTeamScoreLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                AwayTeamScoreLabel.Content = game.Element("linescore").Element("r").Attribute("away").Value.ToString();
                HomeTeamScoreLabel.Content = game.Element("linescore").Element("r").Attribute("home").Value.ToString();
            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Create & show details for - " + AwayTeamLabel.Content.ToString());
            Details gameDetails = new Details(game);
            gameDetails.ShowDialog();
        }
    }
}
