using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoMVVM.ViewModels;

namespace DemoMVVM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new MainViewModel();
    }
}