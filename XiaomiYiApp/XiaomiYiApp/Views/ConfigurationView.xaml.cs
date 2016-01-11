using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.Mvvm;

namespace XiaomiYiApp.Views
{
    public partial class ConfigurationView : PhoneApplicationPage,IView
    {
        public ConfigurationView()
        {
            InitializeComponent(); 
        }

        private  void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
           // object o = picker.se;
            //object o = this.DataContext;

            //await ((XiaomiYiApp.ViewModels.ConfigurationViewModel)this.DataContext).LoadDetailedConfigurationAsync();

            //List<String> ls = new List<string>(new String[]{ "", "aa", "bb"});

            //object l = lbVideo.ItemsSource = ((XiaomiYiApp.ViewModels.ConfigurationViewModel)this.DataContext).VideoParameters;
        }

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    SystemTray.ProgressIndicator.IsVisible = true;
        //    await((XiaomiYiApp.ViewModels.ConfigurationViewModel)this.DataContext).LoadDetailedConfigurationAsync();
        //    SystemTray.ProgressIndicator.IsVisible = false;
        //    base.OnNavigatedTo(e);
        //}
       
    }
}