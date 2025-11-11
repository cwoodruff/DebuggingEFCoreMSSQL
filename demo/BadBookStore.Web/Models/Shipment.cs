using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Shipment
{
    public long ShipmentId { get; set; }

    public long? OrderId { get; set; }

    public string? Carrier { get; set; }

    public string? TrackingNumber { get; set; }

    public string? ShippedOn { get; set; }

    public string? DeliveredOn { get; set; }

    public string? ShipToAddressLine { get; set; }

    public string? Status { get; set; }
}
