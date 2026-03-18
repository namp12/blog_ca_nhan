using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

[Index("Subdomain", Name = "UQ__Blogs__AD95567DDD750F85", IsUnique = true)]
[Index("CustomDomain", Name = "UQ__Blogs__C4BCD080FF6ACAE6", IsUnique = true)]
public partial class Blog
{
    [Key]
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int CurrentThemeId { get; set; }

    public int PlanId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(100)]
    public string Subdomain { get; set; } = null!;

    [StringLength(255)]
    public string? CustomDomain { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? LogoUrl { get; set; }

    [StringLength(500)]
    public string? FaviconUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? Status { get; set; }

    [InverseProperty("Blog")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("Blog")]
    public virtual ICollection<BlogThemeOption> BlogThemeOptions { get; set; } = new List<BlogThemeOption>();

    [InverseProperty("Blog")]
    public virtual ICollection<BlogUser> BlogUsers { get; set; } = new List<BlogUser>();

    [InverseProperty("Blog")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [InverseProperty("Blog")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("CurrentThemeId")]
    [InverseProperty("Blogs")]
    public virtual Theme CurrentTheme { get; set; } = null!;

    [InverseProperty("Blog")]
    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    [ForeignKey("OwnerId")]
    [InverseProperty("Blogs")]
    public virtual User Owner { get; set; } = null!;

    [ForeignKey("PlanId")]
    [InverseProperty("Blogs")]
    public virtual SubscriptionPlan Plan { get; set; } = null!;

    [InverseProperty("Blog")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("Blog")]
    public virtual ICollection<Subscriber> Subscribers { get; set; } = new List<Subscriber>();

    [InverseProperty("Blog")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
