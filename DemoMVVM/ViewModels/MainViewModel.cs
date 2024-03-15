using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
        Page = 1;
        LoadManufacturers();
        LoadDataAsync();
    }
    
    private async Task LoadDataAsync()
    {
        await LoadProducts();
    }

    public async Task LoadProducts()
    {
        try
        {
            IsLoading = true;
            List<Product> list;

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

            var taskList = list
                .Select(async x => new ProductModel()
                {
                    Product = x,
                    HasDiscount = x.Productdiscountamount > 0,
                    ColorBack = x.Productdiscountamount > 0 ? Constants.AdditionalColor : Brushes.White,
                    ProductPhoto = await GetBitmap(x.Productphoto),
                    OldPrice = x.Productdiscountamount > 0 ? $"{x.Productcost:F2}" : "",
                    CurrentPrice = (decimal)(x.Productdiscountamount > 0
                        ? x.Productcost - x.Productcost * x.Productdiscountamount / 100
                        : x.Productcost)
                })
                .ToList();
            var taskResults = await Task.WhenAll(taskList);
            var result = taskResults.ToList();

            if (SelectedSortIndex == 0)
            {
                result = result.OrderBy(result => result.CurrentPrice).ToList();
            }
            else
            {
                result = result.OrderByDescending(result => result.CurrentPrice).ToList();
            }

            _maxCount = result.Count();
            result = result
                .Skip((Page - 1) * _itemOnPage)
                .Take(_itemOnPage)
                .ToList();

            Products = result;

            ProductCount = $"{result.Count} из {StroykaMvvmContext.GetContext().Products.Count()}";
            var pageCount = _maxCount / _itemOnPage;
            PageCount = $"{Page} стр из {(pageCount > 0 ? pageCount : 1)} стр";
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

    private int _page;
    private int _itemOnPage = 10;
    private int _maxCount;

    public int Page
    {
        get => _page;
        set
        {
            _page = value;
            LoadDataAsync();
            OnPropertyChanged();
        }
    }

    public void NextPage()
    {
        var maxPage = _maxCount / 10;
        if (Page + 1 <= maxPage)
        {
            Page++;
        }
    }

    public void PrevPage()
    {
        if (Page - 1 > 0)
        {
            Page--;
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
                Page = 1;
                LoadDataAsync();
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
            LoadDataAsync();
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

    private string _pageCount;

    public string PageCount
    {
        get => _pageCount;
        set
        {
            _pageCount = value;
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
            Page = 1;
            LoadDataAsync();
            OnPropertyChanged();
        }
    }

    private async Task DeleteItemCommand(object item)
    {
        var product = (item as ProductModel).Product;
    }

    private async Task<Bitmap> GetBitmap(string path)
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