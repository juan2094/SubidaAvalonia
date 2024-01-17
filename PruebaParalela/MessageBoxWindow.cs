namespace PruebaParalela;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

public class MessageBoxWindow : Window
{
    private TextBlock _messageText;

    public MessageBoxWindow(string message)
    {
        InitializeComponent();
        _messageText.Text = message;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _messageText = this.FindControl<TextBlock>("MessageText");
    }
}
