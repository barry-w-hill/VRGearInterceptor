using NRepeat;
using System;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using VrGear.Intercepter.UI.Properties;

namespace VrGear.Intercepter.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double messageCount;
        public static TcpProxy _proxy = GetNewProxy();

        private static TcpProxy GetNewProxy()
        {
            return new TcpProxy(new ProxyDefinition()
            {
                ServerAddress = IPAddress.IPv6Any,
                ServerPort = 30322,
                ClientAddress = IPAddress.IPv6Any,
                ClientPort = 63500,
            });
        }

        private bool _hasInitialised;

        private PresentationModel _presentationModel;
        public PresentationModel PresentationModel
        {
            get
            {
                return _presentationModel ?? (_presentationModel = new PresentationModel());
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            if (IsAdministrator())
            {
                AddEvents();
                InitRadioButtons();
                DataContext = PresentationModel;
                _hasInitialised = true;
                RunIntercepter();
            }
            else
            {
                MessageBox.Show("VR-Gear Intercepter needs to run as Admininstrator", "Hold on a minute...");
                this.Close();
            }
        }

        private void AddEvents()
        {
            this.Closing += MainWindow_Closing;

            RadioCustom.Checked += RadioCustom_Changed;
            RadioCustom.Unchecked += RadioCustom_Changed;

            RadioNarrow.Unchecked += RadioCustom_Changed;
            RadioNarrow.Checked += RadioCustom_Changed;
            
            RadioWide.Checked += RadioCustom_Changed;
            RadioWide.Unchecked += RadioCustom_Changed;

            _proxy.ServerDataSentToClient += _proxy_ServerDataSentToClient;
            _proxy.ClientDataSentToServer += _proxy_ClientDataSentToServer;
        }

        private float GetLensSeparationValueInFloat()
        {
            return (float)(GetLensSeparationValue() / 1000);
        }

        private void SetLensSeparation()
        {
            _proxy.LensSeparation = GetLensSeparationValueInFloat();
            PresentationModel.IntercepterStatus = GetLensSeparationValue() + "mm";
        }

        public static bool IsAdministrator()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                    .IsInRole(WindowsBuiltInRole.Administrator);
        }

        void RadioCustom_Changed(object sender, RoutedEventArgs e)
        {
            if (((RadioButton)sender).IsChecked.HasValue && ((RadioButton)sender).IsChecked.Value)
            {
                ((RadioButton)sender).Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ((RadioButton)sender).Foreground = new SolidColorBrush(Colors.Black);
            }
            SetLensSeparation();
        }

        private void InitRadioButtons()
        {
            if (Settings.Default.LensSeparation == (decimal)59.5)
            {
                RadioNarrow.IsChecked = true;
            }
            else if (Settings.Default.LensSeparation == (decimal)69.5)
            {
                RadioWide.IsChecked = true;
            }
            else
            {
                RadioCustom.IsChecked = true;
            }
            PresentationModel.CustomValue = Settings.Default.LensSeparation;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.LensSeparation = GetLensSeparationValue();
            if (_proxy.Running)
            {
                _proxy.Stop();
            }
            Settings.Default.Save();
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        RunIntercepter();
        //        PresentationModel.IntercepterStatus = "Converting 63.5mm -> " + GetLensSeparationValue();
        //        BitcoinHelp.Visibility = System.Windows.Visibility.Visible;
        //        //StartButton.IsEnabled = false;
        //        //StartButton.Content = "Hax with " + GetLensSeparationValue().ToString() + "mm";
        //        RadioStackPanel.Visibility = System.Windows.Visibility.Hidden;
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        PresentationModel.IntercepterStatus = "Refueling";
        //        ReEnabledControls();
        //    }
        //    catch (Exception ex)
        //    {
        //        PresentationModel.IntercepterStatus = "Error: " + ex.Message;
        //        ReEnabledControls();
        //    }
        //}

        private void ReEnabledControls()
        {
            //StartButton.IsEnabled = true;
            //StartButton.Content = "Hack the Planet";
            RadioStackPanel.Visibility = System.Windows.Visibility.Visible;
        }

        private void RunIntercepter()
        {
            if (_hasInitialised)
            {
                if (_proxy.Running)
                {
                    _proxy.Stop();
                    _proxy = GetNewProxy();
                }
                try
                {
                    _proxy.Start();
                }
                catch (Exception ex)
                {
                    if (_proxy.Running)
                    {
                        _proxy.Stop();
                    }
                    PresentationModel.ErrorText = ex.Message;
                    //StartButton.IsEnabled = true;
                }
            }
        }

        public void _proxy_ClientDataSentToServer(object sender, ProxyByteDataEventArgs e)
        {
            if (messageCount + 1 > Int32.MaxValue)
            {
                PresentationModel.IntercepterStatus2 = "Intercepted Messages >>> More than Int32 MaxValue, Achievement Unlocked!";
            }
            {
                PresentationModel.IntercepterStatus2 = "Intercepted Messages >>> " + messageCount + " Messages";
                PresentationModel.IntercepterStatus3 = "Bitcoins mined: " + Math.Round((messageCount / 123), 8);
                PresentationModel.BitcoinVisible = System.Windows.Visibility.Visible;
                PresentationModel.IntercepterStatus4 = "Current threads: " + Process.GetCurrentProcess().Threads.Count;
            }

        }

        public void _proxy_ServerDataSentToClient(object sender, ProxyByteDataEventArgs e)
        {
            messageCount = messageCount + 1;
            PresentationModel.IntercepterStatus2 = "Intercepted Messages >>> " + messageCount + " Messages";
            PresentationModel.IntercepterStatus3 = "Bitcoins mined: " + Math.Round((messageCount / 123),8);
            PresentationModel.BitcoinVisible = System.Windows.Visibility.Visible;
            PresentationModel.IntercepterStatus4 = "Current threads: " + Process.GetCurrentProcess().Threads.Count;
        }

        private decimal GetLensSeparationValue()
        {
            if (RadioNarrow.IsChecked.HasValue && RadioNarrow.IsChecked.Value)
            {
                return (decimal)59.5;
            }
            else if (RadioWide.IsChecked.HasValue && RadioWide.IsChecked.Value)
            {
                return (decimal)69.5;
            }
            else
            {
                return PresentationModel.CustomValue;
            }
        }

        private void DecimalUpDown_GotFocus_1(object sender, RoutedEventArgs e)
        {
            RadioCustom.IsChecked = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new InstructionsPopup().ShowDialog();
        }

        private void BitcoinHelp_PreviewMouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show("Just kidding! []-)");
        }

        private void DecimalUpDown_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SetLensSeparation();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
