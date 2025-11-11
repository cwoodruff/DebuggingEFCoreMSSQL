using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Payment
{
    public long PaymentId { get; set; }

    public long? OrderId { get; set; }

    public double? PaidAmount { get; set; }

    public string? PaidCurrency { get; set; }

    public string? PaidOn { get; set; }

    public string? CardLast4 { get; set; }

    public string? CardHolder { get; set; }

    public string? ProcessorResponse { get; set; }
}
