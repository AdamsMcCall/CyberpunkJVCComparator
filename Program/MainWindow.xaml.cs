using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace Program
{
    /// <summary>
    /// Logique d'interaction pour Mockup.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<GameInfo> _gameList;
        ComparedGames comparedGames;
        Random rnd;
        GameCollection gameCollection;
        int indexBuffer = -1;
        public MainWindow()
        {
            InitializeComponent();
            txt_firstGameName.Opacity = 0;
            sp_firstGameMark.Opacity = 0;
            txt_comparison.Opacity = 0;
            txt_secondGameName.Opacity = 0;
            sp_secondGameMark.Opacity = 0;

            try
            {
                string gameListContent;
                gameListContent = File.ReadAllText("gamelist.json");
                _gameList = JsonConvert.DeserializeObject<List<GameInfo>>(gameListContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            gameCollection = (GameCollection)Resources["GameCollection"];
            _gameList = _gameList.OrderBy(x => x.Name).ToList();
            rnd = new Random();
            comparedGames = new ComparedGames();
            FillGameCollection();
            SetFirstGameToCyberpunk();

            comparedGames.SecondGame = _gameList[rnd.Next(0, _gameList.Count)];
            UpdateComparatorText();

            CB_gameList.SelectionChanged += CB_gameList_SelectionChanged;

            this.DataContext = comparedGames;
        }

        private void FirstGameClick(object sender, RoutedEventArgs e)
        {
            var processInfo = new ProcessStartInfo(comparedGames.FirstGame.Link)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processInfo);
        }

        private void SecondGameClick(object sender, RoutedEventArgs e)
        {
            var processInfo = new ProcessStartInfo(comparedGames.SecondGame.Link)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(processInfo);
        }

        private void BT_restart_Click(object sender, RoutedEventArgs e)
        {
            ReinitializeTextOpacity();

            comparedGames.SecondGame = _gameList[rnd.Next(0, _gameList.Count)];
            UpdateComparatorText();

            SB_gameAnimation.Begin();
        }

        private void ReinitializeTextOpacity()
        {
            txt_firstGameName.BeginAnimation(TextBlock.OpacityProperty, null);
            sp_firstGameMark.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_comparison.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_secondGameName.BeginAnimation(TextBlock.OpacityProperty, null);
            sp_secondGameMark.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_firstGameName.Opacity = 0;
            sp_firstGameMark.Opacity = 0;
            txt_comparison.Opacity = 0;
            txt_secondGameName.Opacity = 0;
            sp_secondGameMark.Opacity = 0;
        }

        private void UpdateComparatorText()
        {
            int firstGrade = Convert.ToInt32(comparedGames.FirstGame.Grade);
            int secondGrade = Convert.ToInt32(comparedGames.SecondGame.Grade);

            if (firstGrade > secondGrade)
                comparedGames.ComparatorText = "a une meilleure note que";
            else if (firstGrade < secondGrade)
                comparedGames.ComparatorText = "a une moins bonne note que";
            else
                comparedGames.ComparatorText = "a la même note que";
        }

        private void FillGameCollection()
        {
            foreach (GameInfo game in _gameList)
                gameCollection.Add(game);
        }

        private void SetFirstGameToCyberpunk()
        {
            int index = _gameList.FindIndex(x => x.Name == "Cyberpunk 2077");

            if (index != -1)
            {
                CB_gameList.SelectedIndex = index;
                comparedGames.FirstGame = _gameList[CB_gameList.SelectedIndex];
            }
            else
            {
                index = rnd.Next(0, _gameList.Count);

                CB_gameList.SelectedIndex = index;
                comparedGames.FirstGame = _gameList[index];
            }
        }

        private void CB_gameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indexBuffer = CB_gameList.SelectedIndex;
        }

        private void CB_gameList_DropDownClosed(object sender, EventArgs e)
        {
            ReinitializeTextOpacity();

            comparedGames.FirstGame = _gameList[indexBuffer];
            indexBuffer = -1;
            UpdateComparatorText();

            SB_gameAnimation.Begin();
        }
    }
}