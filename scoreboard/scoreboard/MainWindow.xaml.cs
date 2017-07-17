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

using System.Net.Http;
using System.IO;
using System.Xml;
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
            InitializeComponent();

            DoStuff();
        }

        private async void DoStuff()
        {
            HttpClient client = new HttpClient();

            string xml = "http://gd2.mlb.com/components/game/mlb/year_2017/month_07/day_14/scoreboard.xml";

            HttpResponseMessage response = await client.GetAsync(xml);

            if (response.IsSuccessStatusCode)
            {
                Game game = new Game();

                gamesStackPanel.Children.Add(game);
            }
        }
    }
}
