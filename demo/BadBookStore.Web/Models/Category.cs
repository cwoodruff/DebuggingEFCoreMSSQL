using System;
using System.Collections.Generic;

namespace BadBookStore.Web.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public string? ParentCategoryName { get; set; }
}