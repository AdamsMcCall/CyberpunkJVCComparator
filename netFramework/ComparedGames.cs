using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace netFramework
{
    public class ComparedGames : INotifyPropertyChanged
    {
        private GameInfo FirstGameValue;
        public GameInfo FirstGame
        {
            get { return FirstGameValue; }
            set
            {
                if (value != FirstGameValue)
                {
                    FirstGameValue = value;
                    OnPropertyChanged("FirstGame");
                }
            }
        }
        private string ComparatorTextValue;
        public string ComparatorText
        {
            get { return ComparatorTextValue; }
            set
            {
                if (value != ComparatorTextValue)
                {
                    ComparatorTextValue = value;
                    OnPropertyChanged("ComparatorText");
                }
            }
        }
        private GameInfo SecondGameValue;
        public GameInfo SecondGame
        {
            get { return SecondGameValue; }
            set
            {
                if (value != SecondGameValue)
                {
                    SecondGameValue = value;
                    OnPropertyChanged("SecondGame");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}