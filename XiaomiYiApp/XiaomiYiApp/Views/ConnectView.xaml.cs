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
    public partial class ConnectView : PhoneApplicationPage, IView
    {
        public ConnectView()
        {
            
            InitializeComponent();
           // INavigate
           /// this.NavigationService.
           /// 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            buttonRec.IsBlinking = !buttonRec.IsBlinking;
        }
    }
}