using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Customer
{
    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? BillingAddressLine { get; set; }

    public string? ShippingAddressLine { get; set; }

    public string? RegisteredOn { get; set; }

    public string? MarketingOptIn { get; set; }
}
