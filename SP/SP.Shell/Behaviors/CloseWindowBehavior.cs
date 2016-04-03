using System.Windows;

using MahApps.Metro.SimpleChildWindow;

namespace SP.Shell.Behaviors
{
    public class CloseWindowBehavior
    {
        public static readonly DependencyProperty CloseRequestedProperty =
            DependencyProperty.RegisterAttached(
                "CloseRequested",
                typeof(bool),
                typeof(CloseWindowBehavior),
                new PropertyMetadata(default(bool), PropertyChangedCallback));

        public static bool GetCloseRequested(UIElement element)
        {
            return (bool)element.GetValue(CloseRequestedProperty);
        }

        public static void SetCloseRequested(UIElement element, bool value)
        {
            element.SetValue(CloseRequestedProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window)
            {
                ((Window)d).Close();
            }
            else if (d is ChildWindow)
            {
                ((ChildWindow)d).Close();
            }
        }
    }
}
