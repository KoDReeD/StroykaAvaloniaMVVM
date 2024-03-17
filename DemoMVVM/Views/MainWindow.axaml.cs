using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DemoMVVM.Models;
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
        Startup();
    }

    private void Startup()
    {
        MainViewModel mvm = DataContext as MainViewModel;
        mvm.WinShowDialog += WinShowDialogAsync;
    }

    private async Task WinShowDialogAsync(object sender, Product e) // Change the return type to Task
    {
        var win = new AddEditWindow
        {
            DataContext = new AddEditViewModel(e)
        };
        await win.ShowDialog(this);
    }
}