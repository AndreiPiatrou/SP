using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;

using MahApps.Metro.SimpleChildWindow.Utils;

namespace SP.Shell.Behaviors
{
    public class ToggleButtonGroup
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
            "Name",
            typeof(string),
            typeof(ToggleButtonGroup),
            new PropertyMetadata(default(string), NamePropertyChangedCallback));

        public static string GetName(UIElement element)
        {
            return (string)element.GetValue(NameProperty);
        }

        public static void SetName(UIElement element, string value)
        {
            element.SetValue(NameProperty, value);
        }

        private static void NamePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            element.Loaded += ElementOnLoaded;
        }

        private static void ElementOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var element = (FrameworkElement)sender;

            foreach (var button in element.FindChildren<ToggleButton>())
            {
                button.Checked += (s, args) =>
                    {
                        foreach (var source in element.FindChildren<ToggleButton>().Where(b => !b.Equals(button)))
                        {
                            source.IsChecked = false;
                        }
                    };
            }
        }
    }
}
