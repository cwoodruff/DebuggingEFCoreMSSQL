using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class OrderLine
{
    public long OrderLineId { get; set; }

    public long? OrderId { get; set; }

    public string? BookTitle { get; set; }

    public int? Quantity { get; set; }

    public double? UnitPrice { get; set; }

    public string? Currency { get; set; }
}
