using System;
using System.Collections.Generic;

namespace DemoMVVM.Models;

public partial class Pickuppoint
{
    public int Pickuppointid { get; set; }

    public string Pickuppointaddres { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
