using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoMVVM.ViewModels;

namespace DemoMVVM.Views;

public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new AuthViewModel();
    }
}