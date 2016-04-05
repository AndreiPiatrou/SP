using System.Windows;
using System.Windows.Controls;

namespace SP.Shell.Behaviors
{
    public class SelectValueOnLoad
    {
        public static readonly DependencyProperty SelectProperty = DependencyProperty.RegisterAttached(
            "Select",
            typeof(bool),
            typeof(SelectValueOnLoad),
            new PropertyMetadata(default(bool), SelectPropertyChangedCallback));

        public static bool GetSelect(UIElement element)
        {
            return (bool)element.GetValue(SelectProperty);
        }

        public static void SetSelect(UIElement element, bool value)
        {
            element.SetValue(SelectProperty, value);
        }

        private static void SelectPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;

            if (textBox == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                textBox.Loaded += TextBoxOnLoaded;
            }
            else
            {
                textBox.Loaded -= TextBoxOnLoaded;
            }
        }

        private static void TextBoxOnLoaded(object s, RoutedEventArgs e)
        {
            ((TextBox)s).SelectAll();
        }
    }
}
