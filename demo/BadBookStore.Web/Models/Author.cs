using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Author
{
    public string AuthorId { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? Bio { get; set; }

    public string? TwitterHandle { get; set; }

    public string? CreatedDate { get; set; }
}