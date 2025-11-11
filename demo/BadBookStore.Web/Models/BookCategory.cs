using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class BookCategory
{
    public string Isbn { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public int? Rank { get; set; }
}
