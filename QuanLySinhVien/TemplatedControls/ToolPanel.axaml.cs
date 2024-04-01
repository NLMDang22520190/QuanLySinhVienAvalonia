using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using ReactiveUI;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace QuanLySinhVien.TemplatedControls
{
    public class ToolPanel : TemplatedControl
    {
        private TextBox _searchTextBox;


        public static readonly StyledProperty<string> CurrentTimeProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(CurrentTime));
        public string CurrentTime
        {
            get { return GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> Command1Property =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(Command1));

        public ReactiveCommand<Unit, Unit> Command1
        {
            get { return GetValue(Command1Property); }
            set { SetValue(Command1Property, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> Command2Property =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(Command2));

        public ReactiveCommand<Unit, Unit> Command2
        {
            get { return GetValue(Command2Property); }
            set { SetValue(Command2Property, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> Command3Property =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(Command3));

        public ReactiveCommand<Unit, Unit> Command3
        {
            get { return GetValue(Command3Property); }
            set { SetValue(Command3Property, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> Command4Property =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(Command4));

        public ReactiveCommand<Unit, Unit> Command4
        {
            get { return GetValue(Command4Property); }
            set { SetValue(Command4Property, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> Command5Property =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(Command5));

        public ReactiveCommand<Unit, Unit> Command5
        {
            get { return GetValue(Command5Property); }
            set { SetValue(Command5Property, value); }
        }

      
    }
}


