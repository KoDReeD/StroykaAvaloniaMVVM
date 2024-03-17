using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoMVVM.Models;
using DemoMVVM.ViewModels;

namespace DemoMVVM.Views;

public partial class AddEditWindow : Window
{
    public AddEditWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}