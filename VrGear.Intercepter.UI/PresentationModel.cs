using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace VrGear.Intercepter.UI
{
    public class PresentationModel : INotifyPropertyChanged
    {
        private decimal _customValue;
        public decimal CustomValue
        {
            get
            {
                return _customValue;
            }
            set
            {
                _customValue = value;
                NotifyPropertyChanged();
            }
        }

        private string _intercepterStatus;
        public string IntercepterStatus
        {
            get
            {
                return _intercepterStatus;
            }
            set
            {
                _intercepterStatus = value;
                NotifyPropertyChanged();
            }
        }

        private string _intercepterStatus2;
        public string IntercepterStatus2
        {
            get
            {
                return _intercepterStatus2;
            }
            set
            {
                _intercepterStatus2 = value;
                NotifyPropertyChanged();
            }
        }

        private string _intercepterStatus3;
        public string IntercepterStatus3
        {
            get
            {
                return _intercepterStatus3;
            }
            set
            {
                _intercepterStatus3 = value;
                NotifyPropertyChanged();
            }
        }

        private string _intercepterStatus4;
        public string IntercepterStatus4
        {
            get
            {
                return _intercepterStatus4;
            }
            set
            {
                _intercepterStatus4 = value;
                NotifyPropertyChanged();
            }
        }
        private string _errorText;
        public string ErrorText
        {
            get
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility _bitcoinVisible = Visibility.Hidden;
        public Visibility BitcoinVisible
        {
            get
            {
                return _bitcoinVisible;
            }
            set
            {
                _bitcoinVisible = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
