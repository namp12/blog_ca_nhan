using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

[Index("CodeName", Name = "UQ__Themes__404488D5B7C9B352", IsUnique = true)]
public partial class Theme
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string CodeName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? PreviewImageUrl { get; set; }

    [StringLength(100)]
    public string? Author { get; set; }

    public bool? IsPremium { get; set; }

    public bool? IsActive { get; set; }

    [InverseProperty("Theme")]
    public virtual ICollection<BlogThemeOption> BlogThemeOptions { get; set; } = new List<BlogThemeOption>();

    [InverseProperty("CurrentTheme")]
    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
