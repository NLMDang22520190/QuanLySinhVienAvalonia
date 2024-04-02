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
        public static readonly StyledProperty<string> CurrentTimeProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(CurrentTime));

        public string CurrentTime
        {
            get { return GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> AddCommandProperty =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(AddCommand));

        public ReactiveCommand<Unit, Unit> AddCommand
        {
            get { return GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> RemoveCommandProperty =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(RemoveCommand));

        public ReactiveCommand<Unit, Unit> RemoveCommand
        {
            get { return GetValue(RemoveCommandProperty); }
            set { SetValue(RemoveCommandProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> SaveCommandProperty =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(SaveCommand));

        public ReactiveCommand<Unit, Unit> SaveCommand
        {
            get { return GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> UndoCommandProperty =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(UndoCommand));

        public ReactiveCommand<Unit, Unit> UndoCommand
        {
            get { return GetValue(UndoCommandProperty); }
            set { SetValue(UndoCommandProperty, value); }
        }

        public static readonly StyledProperty<ReactiveCommand<Unit, Unit>> RedoCommandProperty =
            AvaloniaProperty.Register<ToolPanel, ReactiveCommand<Unit, Unit>>(nameof(RedoCommand));

        public ReactiveCommand<Unit, Unit> RedoCommand
        {
            get { return GetValue(RedoCommandProperty); }
            set { SetValue(RedoCommandProperty, value); }
        }

        public static readonly StyledProperty<string> AddTooltipProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(AddTooltip));

        public string AddTooltip
        {
            get { return GetValue(AddTooltipProperty); }
            set { SetValue(AddTooltipProperty, value); }
        }

        public static readonly StyledProperty<string> RemoveTooltipProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(RemoveTooltip));

        public string RemoveTooltip
        {
            get { return GetValue(RemoveTooltipProperty); }
            set { SetValue(RemoveTooltipProperty, value); }
        }

        public static readonly StyledProperty<string> SaveTooltipProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(SaveTooltip));

        public string SaveTooltip
        {
            get { return GetValue(SaveTooltipProperty); }
            set { SetValue(SaveTooltipProperty, value); }
        }

        public static readonly StyledProperty<string> UndoTooltipProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(UndoTooltip), "Undo");

        public string UndoTooltip
        {
            get { return GetValue(UndoTooltipProperty); }
            set { SetValue(UndoTooltipProperty, value); }
        }

        public static readonly StyledProperty<string> RedoTooltipProperty =
            AvaloniaProperty.Register<ToolPanel, string>(nameof(RedoTooltip), "Redo");

        public string RedoTooltip
        {
            get { return GetValue(RedoTooltipProperty); }
            set { SetValue(RedoTooltipProperty, value); }
        }
    }
}