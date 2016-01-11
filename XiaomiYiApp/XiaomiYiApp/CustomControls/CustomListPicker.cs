using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace XiaomiYiApp.CustomControls
{
    public class CustomListPicker : Microsoft.Phone.Controls.ListPicker
    {
        public static readonly DependencyProperty SelectedValue =
    DependencyProperty.Register( "IsSpinning", typeof(Boolean),   typeof(CustomListPicker), null);


        public bool IsSpinning
        {
            get { return (bool)GetValue(SelectedValue); }
            set { SetValue(SelectedValue, value); }
        }

        public CustomListPicker()
        {
            this.SelectionChanged += CustomListPicker_SelectionChanged;
        }

        void CustomListPicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }

       
      

    }
}
