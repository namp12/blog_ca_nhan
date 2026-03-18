using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

[Index("Username", Name = "UQ__Users__536C85E4C358708A", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D10534DD0219EB", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [StringLength(255)]
    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsPlatformAdmin { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("User")]
    public virtual ICollection<BlogUser> BlogUsers { get; set; } = new List<BlogUser>();

    [InverseProperty("Owner")]
    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    [InverseProperty("User")]
    public virtual ICollection<Medium> Media { get; set; } = new List<Medium>();

    [InverseProperty("Author")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [ForeignKey("UserId")]
    [InverseProperty("Users")]
    public virtual ICollection<Post> PostsNavigation { get; set; } = new List<Post>();
}
