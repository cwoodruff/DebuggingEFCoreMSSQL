using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Inventory
{
    public string WarehouseCode { get; set; } = null!;

    public string BookIsbn { get; set; } = null!;

    public string? QuantityOnHand { get; set; }

    public string? ReorderLevel { get; set; }

    public string? LocationNote { get; set; }
}
