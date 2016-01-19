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
            set { SetValue(IsBlinkingProperty, value); }
        }

        private static void IsBlinkingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(((RecordButton)d), (Boolean)e.NewValue ? "Blinking" : "Normal",false);
             
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (!((Boolean)e.NewValue) && IsBlinking)
            {
                VisualStateManager.GoToState(this, "Blinking" , false);
            }
        }
    }
}
