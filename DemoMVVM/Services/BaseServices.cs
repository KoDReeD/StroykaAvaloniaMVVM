using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace DemoMVVM.Services;

public static class BaseServices
{
    public static async Task<Bitmap> GetBitmap(string path)
    {
        try
        {
            var bytes = File.ReadAllBytes(@"..\..\..\Resources\ProductPhotos\" + path);
            using var memoryStream = new MemoryStream(bytes);
            var bitmap = new Bitmap(memoryStream);
            return bitmap;
        }
        catch (Exception e)
        {
            var bytes = File.ReadAllBytes(@"..\..\..\Resources\picture.png");
            using var memoryStream = new MemoryStream(bytes);
            var bitmap = new Bitmap(memoryStream);
            return bitmap;
        }
    }
}