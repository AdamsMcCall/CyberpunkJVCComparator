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

namespace netCore
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
        Func<int>[] GetGameId = new Func<int>[4];
        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
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

            GetGameId[0] = GetRandomGameId;
            GetGameId[1] = GetGreaterGameId;
            GetGameId[2] = GetEqualGameId;
            GetGameId[3] = GetLowerGameId;

            gameCollection = (GameCollection)Resources["GameCollection"];
            _gameList = _gameList.OrderBy(x => x.Name).ToList();
            rnd = new Random();
            comparedGames = new ComparedGames();
            FillGameCollection();
            SetFirstGameToCyberpunk();

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
            ReinitializeTextOpacity();

            comparedGames.FirstGame = _gameList[CB_gameList.SelectedIndex];
            comparedGames.SecondGame = _gameList[GetGameId[CB_secondGameChoice.SelectedIndex]()];
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

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private int GetRandomGameId()
        {
            return rnd.Next(0, _gameList.Count);
        }

        private int GetGreaterGameId()
        {
            int id = rnd.Next(0, _gameList.Count);

            if (Convert.ToInt32(comparedGames.FirstGame.Grade) == 20)
                while (Convert.ToInt32(_gameList[id].Grade) < Convert.ToInt32(comparedGames.FirstGame.Grade))
                    id = rnd.Next(0, _gameList.Count);
            else
                while (Convert.ToInt32(_gameList[id].Grade) <= Convert.ToInt32(comparedGames.FirstGame.Grade))
                    id = rnd.Next(0, _gameList.Count);
            return id;
        }
        
        private int GetEqualGameId()
        {
            int id = rnd.Next(0, _gameList.Count);

            while (Convert.ToInt32(_gameList[id].Grade) != Convert.ToInt32(comparedGames.FirstGame.Grade))
                id = rnd.Next(0, _gameList.Count);
            return id;
        }

        private int GetLowerGameId()
        {
            int id = rnd.Next(0, _gameList.Count);

            if (Convert.ToInt32(comparedGames.FirstGame.Grade) == 0)
                while (Convert.ToInt32(_gameList[id].Grade) > Convert.ToInt32(comparedGames.FirstGame.Grade))
                id = rnd.Next(0, _gameList.Count);
            else
                while (Convert.ToInt32(_gameList[id].Grade) >= Convert.ToInt32(comparedGames.FirstGame.Grade))
                    id = rnd.Next(0, _gameList.Count);
            return id;
        }
    }
}