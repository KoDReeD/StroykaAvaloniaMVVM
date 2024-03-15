using System;
using System.Collections.Generic;

namespace DemoMVVM.Models;

public partial class Orderstatus
{
    public int Orderstatusid { get; set; }

    public string Orderstatusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
