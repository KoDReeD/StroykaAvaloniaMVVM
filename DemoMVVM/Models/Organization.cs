using System;
using System.Collections.Generic;

namespace DemoMVVM.Models;

public partial class Organization
{
    public int Organizationid { get; set; }

    public string Organizationname { get; set; } = null!;

    public virtual ICollection<Product> ProductProductmanufacturerNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductProductvendorNavigations { get; set; } = new List<Product>();
}
