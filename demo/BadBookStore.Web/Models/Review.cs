using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Review
{
    public long ReviewId { get; set; }

    public string? Isbn { get; set; }

    public string? BookTitle { get; set; }

    public string? CustomerEmail { get; set; }

    public int? Rating { get; set; }

    public string? ReviewText { get; set; }

    public string? ReviewDate { get; set; }
}