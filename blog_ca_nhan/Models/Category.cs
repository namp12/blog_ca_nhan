using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(150)]
    public string Slug { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? OrderIndex { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("Categories")]
    public virtual Blog Blog { get; set; } = null!;

    [InverseProperty("Parent")]
    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Category? Parent { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
