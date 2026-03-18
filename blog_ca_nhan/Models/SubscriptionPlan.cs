using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class SubscriptionPlan
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }

    public int? MaxPosts { get; set; }

    [Column("MaxStorageMB")]
    public long? MaxStorageMb { get; set; }

    public bool? HasAdFree { get; set; }

    public bool? HasCustomDomain { get; set; }

    [InverseProperty("Plan")]
    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
