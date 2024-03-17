using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using DemoMVVM.Context;
using DemoMVVM.Models;
using DemoMVVM.Services;

namespace DemoMVVM.ViewModels;



public class AddEditViewModel : INotifyPropertyChanged
{
    public AddEditViewModel(Product product)
    {
        if (product != null)
        {
            IsEdit = true;
            Product = product;
            SelectedManufacturer = Product.ProductmanufacturerNavigation;
            SelectedVendor = Product.ProductvendorNavigation;
            SelectedCategory = Product.ProductcategoryNavigation;
            SelectedUnitMeasurement = Product.Productunitmeasurement;
        }
        else
        {
            Product = new Product();
        }
        
        SetOrganizations();
        SetUnitMeasurements();
        SetCategoryes();
        SetImage();
    }

    private Product _product;
    private bool _isEdit = false;
    private Bitmap _bitmapMainImage;
    private Organization _selectedManufacturer;
    private Organization _selectedVendor;
    private Category _selectedCategory;
    private string _selectedUnitMeasurement;

    public Product Product
    {
        get => _product; 
        set { _product = value; OnPropertyChanged(); }
    }

    public bool IsEdit
    {
        get => _isEdit;
        set { _isEdit = value; OnPropertyChanged(); }
    }
    
    public Bitmap BitmapMainImage
    {
        get => _bitmapMainImage;
        set { _bitmapMainImage = value; OnPropertyChanged(); }
    }
    public Organization SelectedManufacturer
    {
        get => _selectedManufacturer;
        set { _selectedManufacturer = value; OnPropertyChanged(); }
    } 
    public Organization SelectedVendor
    {
        get => _selectedVendor;
        set { _selectedVendor = value; OnPropertyChanged(); }
    }
    public Category SelectedCategory
    {
        get => _selectedCategory;
        set { _selectedCategory = value; OnPropertyChanged(); }
    } 
    
    public string SelectedUnitMeasurement
    {
        get => _selectedUnitMeasurement;
        set { _selectedUnitMeasurement = value; OnPropertyChanged(); }
    }

    public List<Organization> Organizations { get; set; } = new List<Organization>();
    public List<Category> Categoryes { get; set; } = new List<Category>();
    public List<string> UnitMeasurements { get; set; }

    private async void UploadPhoto()
    {
        
    }
    
    private void DeletePhoto()
    {
        
    }
    
    private void SetUnitMeasurements()
    {
        var list = new List<string>() { "шт." };
        UnitMeasurements = list;
    }
    private async void SetImage()
    {
        var bitmapProduct = await BaseServices.GetBitmap(Product.Productphoto);
        BitmapMainImage = bitmapProduct;
    }

    private void SetOrganizations()
    {
        var list = StroykaMvvmContext.GetContext().Organizations.ToList();
        Organizations = list;
    } 
    
    private void SetCategoryes()
    {
        var list = StroykaMvvmContext.GetContext().Categories.ToList();
        Categoryes = list;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}