using System;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using SP.Shell.Commands;

namespace SP.Shell.ViewModel
{
    public class EditStringViewModel : ViewModelBase
    {
        private bool closeRequested;
        private string value;

        public EditStringViewModel(Action<string> applyAction, string valueToEdit)
        {
            Value = valueToEdit;
            Apply = new GuiListenCommand(
                o =>
                    {
                        applyAction(Value);
                        CloseRequested = true;
                    },
                o => true);
        }

        public bool CloseRequested
        {
            get
            {
                return closeRequested;
            }

            set
            {
                closeRequested = value;
                RaisePropertyChanged(() => CloseRequested);
            }
        }

        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public ICommand Apply { get; private set; }
    }
}
