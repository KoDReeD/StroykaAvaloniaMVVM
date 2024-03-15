using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DemoMVVM.Context;
using DemoMVVM.Models;
using DemoMVVM.Models.ModelsView;
using Microsoft.EntityFrameworkCore;

namespace DemoMVVM.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public MainViewModel()
    {
        LoadManufacturers();
        LoadProducts();
    }

    public async void LoadProducts()
    {
        try
        {
            IsLoading = true;
            List<Product> list = new List<Product>();

            //  определённого
            if (SelectedManufacturer != null && SelectedManufacturer.Organizationid != 0)
            {
                list = await StroykaMvvmContext.GetContext().Products
                    .Where(x => x.Productmanufacturer == SelectedManufacturer.Organizationid).ToListAsync();
            }
            // всех
            else
            {
                list = await StroykaMvvmContext.GetContext().Products.ToListAsync();
            }

            var arr = SeachText?.ToLower().Split(" ").Where(x => x != " " && x != "").ToArray();

            if (SeachText != null && arr.Length > 0)
            {
                for (int i = 0; i < arr.Count(); i++)
                {
                    list = list.Where(x =>
                        x.Productname != null && x.Productname.ToLower().Contains(arr[i]) ||
                        x.Productdescription != null && x.Productdescription.ToLower().Contains(arr[i]) ||
                        x.Productcost.ToString().Contains(arr[i])).ToList();
                }
            }
            
            var result = list
                .Select(x => new ProductModel()
                {
                    Product = x,
                    HasDiscount = x.Productdiscountamount > 0,
                    ColorBack = x.Productdiscountamount > 0 ? Brushes.LightGreen : Brushes.White,
                    ProductPhoto = GetBitmap(x.Productphoto),
                    OldPrice = x.Productdiscountamount > 0 ? $"{x.Productcost:F2}" : "",
                    CurrentPrice = (decimal)(x.Productdiscountamount > 0 ? x.Productcost - x.Productcost * x.Productdiscountamount / 100 : x.Productcost)
                })
                .ToList();

            if (SelectedSortIndex == 0)
            {
                result = result.OrderBy(result => result.CurrentPrice).ToList();
            }
            else
            {
                result = result.OrderByDescending(result => result.CurrentPrice).ToList();
            }

            Products = result;
            
            ProductCount = $"{result.Count} из {StroykaMvvmContext.GetContext().Products.Count()}";
            IsLoading = false;
        }
        catch (Exception e)
        {
           
        }
    }

    private List<ProductModel> _products;
    public List<ProductModel> Products
    {
        get => _products;
        set
        {
            if (value != null)
            {
                _products = value;
                OnPropertyChanged();
            }
        }
    }
    
    public ObservableCollection<Organization> Manufacturers { get; set; } = new ObservableCollection<Organization>();
    
    private Organization _selectedManufacturer;
    public Organization SelectedManufacturer
    {
        get => _selectedManufacturer;
        set
        {
            if (value != null)
            {
                _selectedManufacturer = value;
                LoadProducts();
                OnPropertyChanged();
            }
        }
    }
    
    private int _selectedSortIndex = 0;
    public int SelectedSortIndex
    {
        get => _selectedSortIndex;
        set
        {
            _selectedSortIndex = value;
            LoadProducts();
            OnPropertyChanged();
        }
    }
    

    private string _productCount;

    public string ProductCount
    {
        get => _productCount;
        set
        {
            _productCount = value;
            OnPropertyChanged();
        }
    }
    
    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    private string _seachText = "";
    public string SeachText
    {
        get => _seachText;
        set
        {
            _seachText = value;
            LoadProducts();
            OnPropertyChanged();
        }
    }

    private async Task DeleteItemCommand(object item)
    {
        var product = (item as ProductModel).Product;
    }

    private static Bitmap GetBitmap(string path)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path)) throw new Exception();

            var bit = new Bitmap(@"..\..\..\Resources\ProductPhotos\" + path);
            return bit;
        }
        catch (Exception e)
        {
            var bit = new Bitmap(@"..\..\..\Resources\picture.png");
            return bit;
        }
    }

    private async void LoadManufacturers()
    {
        try
        {
            var list = await StroykaMvvmContext.GetContext().Organizations.ToListAsync();
            list.Insert(0, new Organization()
            {
                Organizationid = 0,
                Organizationname = "Все производители"
            });
        
            foreach (var item in list)
            {
                Manufacturers.Add(item);
            }

            SelectedManufacturer = list[0];
        }
        catch (Exception e)
        {
            
        }
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