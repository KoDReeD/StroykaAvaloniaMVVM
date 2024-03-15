using System;
using System.Collections.Generic;

namespace DemoMVVM.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public string Productunitmeasurement { get; set; } = null!;

    public decimal Productcost { get; set; }

    public short? Productmaxdiscountamount { get; set; }

    public int Productmanufacturer { get; set; }

    public int Productvendor { get; set; }

    public int Productcategory { get; set; }

    public short? Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public string Productdescription { get; set; } = null!;

    public string? Productphoto { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Category ProductcategoryNavigation { get; set; } = null!;

    public virtual Organization ProductmanufacturerNavigation { get; set; } = null!;

    public virtual Organization ProductvendorNavigation { get; set; } = null!;
}
