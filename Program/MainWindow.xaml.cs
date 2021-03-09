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

namespace Program
{
    /// <summary>
    /// Logique d'interaction pour Mockup.xaml
    /// </summary>
    public partial class Mockup : Window
    {
        List<GameInfo> _gameList;
        ComparedGames comparedGames;
        Random rnd;
        public Mockup()
        {
            InitializeComponent();
            txt_firstGameName.Opacity = 0;
            sp_firstGameMark.Opacity = 0;
            txt_comparison.Opacity = 0;
            txt_secondGameName.Opacity = 0;
            sp_secondGameMark.Opacity = 0;

            string gameListContent;

            try
            {
                gameListContent = File.ReadAllText("gamelist.json");
                _gameList = JsonConvert.DeserializeObject<List<GameInfo>>(gameListContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }

            rnd = new Random();
            comparedGames = new ComparedGames();

            comparedGames.FirstGame = new GameInfo
            {
                Name = "Cyberpunk 2077",
                Grade = "17",
                Link = "https://www.jeuxvideo.com/jeux/jeu-79026/"
            };
            comparedGames.SecondGame = _gameList[rnd.Next(0, _gameList.Count)];
            UpdateComparatorText();

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

            comparedGames.SecondGame = _gameList[rnd.Next(0, _gameList.Count)];
            UpdateComparatorText();

            SB_gameAnimation.Begin();
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
    }
}
