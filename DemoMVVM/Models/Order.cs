using System;
using System.Collections.Generic;

namespace DemoMVVM.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public DateOnly Orderdate { get; set; }

    public DateOnly Orderdeliverydate { get; set; }

    public int Orderpickuppoint { get; set; }

    public string? Clientfio { get; set; }

    public int Code { get; set; }

    public int Orderstatus { get; set; }

    public virtual Pickuppoint OrderpickuppointNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; set; } = new List<Orderproduct>();

    public virtual Orderstatus OrderstatusNavigation { get; set; } = null!;
}
