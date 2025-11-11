using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Order
{
    public long OrderId { get; set; }

    public string? CustomerEmail { get; set; }

    public string? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public double? OrderTotal { get; set; }

    public string? CouponCode { get; set; }

    public string? Meta { get; set; }
}
