using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SP.Shell.Behaviors
{
    public class GenericDataGridColumnsBinder
    {
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.RegisterAttached(
            "DataSource",
            typeof(ObservableCollection<ObservableCollection<string>>),
            typeof(GenericDataGridColumnsBinder),
            new PropertyMetadata(default(ObservableCollection<ObservableCollection<string>>), PropertyChangedCallback));

        public static ObservableCollection<ObservableCollection<string>> GetDataSource(UIElement element)
        {
            return (ObservableCollection<ObservableCollection<string>>)element.GetValue(DataSourceProperty);
        }

        public static void SetDataSource(UIElement element, ObservableCollection<ObservableCollection<string>> value)
        {
            element.SetValue(DataSourceProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var data = e.NewValue as ObservableCollection<ObservableCollection<string>>;

            BindData(dataGrid, data);
        }

        private static void BindData(DataGrid dataGrid, ObservableCollection<ObservableCollection<string>> data)
        {
            dataGrid.Columns.Clear();
            if (data == null)
            {
                return;
            }

            foreach (var columns in data)
            {
                dataGrid.Columns.Add(new DataGridTextColumn());
            }
        }
    }
}
