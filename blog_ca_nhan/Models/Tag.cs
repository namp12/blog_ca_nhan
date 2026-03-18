using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Tag
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Slug { get; set; } = null!;

    [ForeignKey("BlogId")]
    [InverseProperty("Tags")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("TagId")]
    [InverseProperty("Tags")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
