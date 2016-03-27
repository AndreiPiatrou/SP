using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
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
                dataGrid.SelectedCellsChanged += (sender, args) =>
                    {
                        if (!args.AddedCells.Any())
                        {
                            data.SelectedRow = -1;
                            data.SelectedHeader = -1;
                            Debug.WriteLine("Selection:" + data.SelectedRow + ", " + data.SelectedHeader);

                            return;
                        }

                        data.SelectedRow = data.Records.IndexOf((ObservableCollection<string>)args.AddedCells[0].Item);
                        data.SelectedHeader = args.AddedCells[0].Column.DisplayIndex;

                        Debug.WriteLine("Selection:" + data.SelectedRow + ", " + data.SelectedHeader);
                    };
            }
        }

        private static void HandleHeadersCollectionChange(NotifyCollectionChangedEventArgs e, DataGrid dataGrid)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var indexer = e.NewStartingIndex;
                foreach (var t in e.NewItems)
                {
                    dataGrid.Columns.Add(CreateDataGridColumn(t.ToString(), indexer));
                    ++indexer;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                dataGrid.Columns.RemoveAt(e.OldStartingIndex);
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
                dataGrid.Columns.Add(CreateDataGridColumn(column, indexer));
                ++indexer;
            }
        }

        private static DataGridColumn CreateDataGridColumn(string column, int index)
        {
            return new DataGridTextColumn
            {
                Header = column,
                Binding = new Binding("[" + index + "]")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                },
                IsReadOnly = false
            };
        }
    }
}
