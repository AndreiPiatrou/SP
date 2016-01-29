using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using SP.Shell.Models;

namespace SP.Shell.Behaviors
{
    public class GenericDataGridColumnsBinder
    {
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.RegisterAttached(
            "DataSource",
            typeof(RecordsCollection),
            typeof(GenericDataGridColumnsBinder),
            new PropertyMetadata(default(RecordsCollection), PropertyChangedCallback));

        public static RecordsCollection GetDataSource(UIElement element)
        {
            return (RecordsCollection)element.GetValue(DataSourceProperty);
        }

        public static void SetDataSource(UIElement element, RecordsCollection value)
        {
            element.SetValue(DataSourceProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid)d;
            var data = (RecordsCollection)e.NewValue;

            BindData(dataGrid, data);
            if (data != null)
            {
                data.Headers.CollectionChanged += (sender, args) => HandleHeadersCollectionChange(args, dataGrid);
                dataGrid.CellEditEnding += (sender, args) => data.UpdateRowsAndHeaders();
                dataGrid.RowEditEnding += (sender, args) => data.UpdateRowsAndHeaders();
            }
        }

        private static void HandleHeadersCollectionChange(NotifyCollectionChangedEventArgs e, DataGrid dataGrid)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var indexer = e.NewStartingIndex;
                foreach (var t in e.NewItems)
                {
                    dataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = t.ToString(),
                        Binding = new Binding("[" + indexer + "]")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
                        },
                    });
                    ++indexer;
                }
            }
        }

        private static void BindData(DataGrid dataGrid, RecordsCollection data)
        {
            dataGrid.Columns.Clear();
            if (data == null)
            {
                return;
            }
            
            dataGrid.SetBinding(
                ItemsControl.ItemsSourceProperty,
                new Binding("Records.Records")
                    {
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

            var indexer = 0;
            foreach (var column in data.Headers)
            {
                dataGrid.Columns.Add(
                    new DataGridTextColumn
                    {
                        Header = column,
                        Binding = new Binding("[" + indexer + "]")
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                        }
                    });
                ++indexer;
            }
        }
    }
}
