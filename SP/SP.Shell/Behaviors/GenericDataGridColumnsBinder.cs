using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using GalaSoft.MvvmLight.Messaging;

using Microsoft.Practices.ServiceLocation;

using SP.Shell.Messages;
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
                dataGrid.SelectedCellsChanged += (sender, args) => DataGridSelectionChanged(args, data);
            }
        }

        private static void DataGridSelectionChanged(SelectedCellsChangedEventArgs args, RecordsCollection data)
        {
            if (!args.AddedCells.Any())
            {
                data.SelectedRow = -1;
                data.SelectedHeader = -1;

                return;
            }

            data.SelectedRow = data.Records.IndexOf((ObservableCollection<string>)args.AddedCells[0].Item);
            data.SelectedHeader = args.AddedCells[0].Column.DisplayIndex;

            ServiceLocator.Current.GetInstance<Messenger>().Send(new DataGridSelectionChangedMessage(data));
        }

        private static void HandleHeadersCollectionChange(NotifyCollectionChangedEventArgs e, DataGrid dataGrid)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var indexer = e.NewStartingIndex;
                    foreach (var t in e.NewItems)
                    {
                        dataGrid.Columns.Insert(indexer, CreateDataGridColumn(t.ToString(), indexer));
                        ++indexer;
                    }

                    UpdateBindings(dataGrid);

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

        private static void UpdateBindings(DataGrid dataGrid)
        {
            var index = 0;
            foreach (var column in dataGrid.Columns.Cast<DataGridTextColumn>())
            {
                column.Binding = CreateBinding(index++);
            }
        }

        private static void BindData(DataGrid dataGrid, RecordsCollection data)
        {
            dataGrid.Columns.Clear();
            if (data == null)
            {
                return;
            }

            BindRows(dataGrid);
            BindHeaders(dataGrid, data);
        }

        private static void BindRows(DataGrid dataGrid)
        {
            dataGrid.SetBinding(
                ItemsControl.ItemsSourceProperty,
                new Binding("Records.Records")
                {
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        }

        private static void BindHeaders(DataGrid dataGrid, RecordsCollection data)
        {
            var indexer = 0;

            foreach (var column in data.Headers)
            {
                dataGrid.Columns.Add(CreateDataGridColumn(column, indexer));
                ++indexer;
            }
        }

        private static DataGridTextColumn CreateDataGridColumn(string column, int index)
        {
            return new DataGridTextColumn
            {
                Header = column,
                Binding = CreateBinding(index),
                IsReadOnly = false
            };
        }

        private static BindingBase CreateBinding(int index)
        {
            return new Binding("[" + index + "]")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
            };
        }
    }
}
