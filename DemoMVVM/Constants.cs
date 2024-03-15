using Avalonia.Media;

namespace DemoMVVM;

public class Constants
{
    public static IBrush MainColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 255, 255));
    public static IBrush AdditionalColor { get; set; } = new SolidColorBrush(Color.FromRgb(118, 227, 131));
    public static IBrush AttentionalColor { get; set; } = new SolidColorBrush(Color.FromRgb(73, 140,81));
}