using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Mockup()
        {
            InitializeComponent();
            txt_firstGameName.Opacity = 0;
            txt_firstGameMark.Opacity = 0;
            txt_comparison.Opacity = 0;
            txt_secondGameName.Opacity = 0;
            txt_secondGameMark.Opacity = 0;
        }

        private void BT_restart_Click(object sender, RoutedEventArgs e)
        {
            //SB_gameAnimation.Remove();
            txt_firstGameName.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_firstGameMark.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_comparison.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_secondGameName.BeginAnimation(TextBlock.OpacityProperty, null);
            txt_secondGameMark.BeginAnimation(TextBlock.OpacityProperty, null);

            txt_firstGameName.Opacity = 0;
            txt_firstGameMark.Opacity = 0;
            txt_comparison.Opacity = 0;
            txt_secondGameName.Opacity = 0;
            txt_secondGameMark.Opacity = 0;
            SB_gameAnimation.Begin();
        }
    }
}
