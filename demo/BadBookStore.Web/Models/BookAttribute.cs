using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class BookAttribute
{
    public long AttributeId { get; set; }

    public string? Isbn { get; set; }

    public string? AttributeName { get; set; }

    public string? AttributeType { get; set; }

    public string? AttributeValue { get; set; }
}