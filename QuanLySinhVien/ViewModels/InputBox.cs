using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.ViewModels
{
    public class InputBox : Window
    {
        private TextBox input;
        private Button confirmButton;
        private TaskCompletionSource<string> taskCompletionSource;

        public InputBox(string title, string message)
        {
            this.Title = title;
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(new TextBlock { Text = message });
            input = new TextBox();
            stackPanel.Children.Add(input);

            confirmButton = new Button { Content = "Xác nhận" };
            confirmButton.Click += ConfirmButton_Click;
            stackPanel.Children.Add(confirmButton);

            this.Content = stackPanel;
            this.Width = 300;
            this.Height = 200;
        }

        private void ConfirmButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            taskCompletionSource.SetResult(input.Text);
            this.Close();
        }

        public Task<string> ShowWindowDialogAsync(Window parentWindow)
        {
            taskCompletionSource = new TaskCompletionSource<string>();
            this.ShowDialog(parentWindow);
            return taskCompletionSource.Task;
        }
    }


}
