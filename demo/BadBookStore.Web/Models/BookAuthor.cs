using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class BookAuthor
{
    public string BookTitle { get; set; } = null!;

    public string AuthorDisplayName { get; set; } = null!;

    public string? Role { get; set; }

    public int? SortOrder { get; set; }
}
