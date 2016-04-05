using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using SP.Shell.Models;
using SP.Shell.ViewModel;

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
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var style = dataGrid.FindResource("MaterialDesignDataGridColumnHeader") as Style;
                    var indexer = e.NewStartingIndex;
                    foreach (var t in e.NewItems)
                    {
                        dataGrid.Columns.Add(CreateDataGridColumn(t.ToString(), indexer, style));
                        ++indexer;
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    dataGrid.Columns.RemoveAt(e.OldStartingIndex);
                    break;
                default:
                    dataGrid.Columns.Clear();
                    BindHeaders(dataGrid, GetDataSource(dataGrid));
                    break;
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

            BindHeaders(dataGrid, data);
        }

        private static void BindHeaders(DataGrid dataGrid, RecordsCollection data)
        {
            var style = dataGrid.FindResource("MaterialDesignDataGridColumnHeader") as Style;
            var indexer = 0;

            foreach (var column in data.Headers)
            {
                dataGrid.Columns.Add(CreateDataGridColumn(column, indexer, style));
                ++indexer;
            }
        }

        private static DataGridColumn CreateDataGridColumn(string column, int index, Style baseStyle)
        {
            var element = new DataGridTextColumn
            {
                Header = column,
                Binding = new Binding("[" + index + "]")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                },
                IsReadOnly = false
            };
            
            //var style = new Style(typeof(DataGridColumnHeader), baseStyle);
            //style.Setters.Add(new Setter(ToolTipService.ToolTipProperty, column));
            //element.HeaderStyle = style;

            return element;
        }
    }
}
