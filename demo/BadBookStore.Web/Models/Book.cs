using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? SubTitle { get; set; }

    public string? AuthorId { get; set; }

    public double? ListPrice { get; set; }

    public string? CurrencyCode { get; set; }

    public string? PublishedOn { get; set; }

    public string? CategoryCsv { get; set; }

    public string? IsActive { get; set; }

    public string? ExtraData { get; set; }
}
