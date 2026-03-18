using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class BlogThemeOption
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int ThemeId { get; set; }

    [StringLength(100)]
    public string OptionKey { get; set; } = null!;

    public string? OptionValue { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("BlogThemeOptions")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("ThemeId")]
    [InverseProperty("BlogThemeOptions")]
    public virtual Theme Theme { get; set; } = null!;
}
