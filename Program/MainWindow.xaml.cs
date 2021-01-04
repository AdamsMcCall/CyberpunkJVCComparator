using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<GameInfo> _gameList;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string gameListContent;

            try
            {
                gameListContent = File.ReadAllText("gamelist.json");
                _gameList = JsonConvert.DeserializeObject<List<GameInfo>>(gameListContent);
                LBL_test.Content = _gameList.Count.ToString() + " games listed";
            }
            catch (Exception ex)
            {
                LBL_error_content.Text = "ERROR: " + ex.Message;
                LBL_test.Content = "Couldn't load the game list";
            }
        }
    }
}
