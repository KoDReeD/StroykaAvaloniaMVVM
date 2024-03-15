using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore.Storage;

namespace DemoMVVM.Models.ModelsView;

public class ProductModel
{
    public Product Product { get; set; }
    public IBrush ColorBack { get; set; }
    public Bitmap ProductPhoto { get; set; }
    
    public decimal CurrentPrice { get; set; }
    public string? OldPrice { get; set; }
    public bool HasDiscount { get; set; }
    
    
}