using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace XiaomiYiApp.CustomControls
{
    public class RecordButton : Button
    {
        public static readonly DependencyProperty IsBlinkingProperty =
    DependencyProperty.Register("IsBlinking", typeof(Boolean), typeof(RecordButton), new PropertyMetadata(false, IsBlinkingPropertyChangedCallback));


        public bool IsBlinking
        {
            get { return (bool)GetValue(IsBlinkingProperty); }
            set 
            {
                if (IsEnabled || !value)
                {
                    SetValue(IsBlinkingProperty, value);
                }
            }
        }

        public RecordButton()
        {
            this.IsEnabledChanged += RecordButton_IsEnabledChanged;
            this.Loaded += RecordButton_Loaded;
        }

        void RecordButton_Loaded(object sender, RoutedEventArgs e)
        {
            IsBlinking = !IsBlinking;
            IsBlinking = !IsBlinking;
        }

        void RecordButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Boolean)e.NewValue)
            {
                IsBlinking = false;
            }
        }

        private static void IsBlinkingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           // VisualStateManager.GoToState(((RecordButton)d), (Boolean)e.NewValue ? "Blinking" : "Normal",false);
             
        }
      
    }
}
