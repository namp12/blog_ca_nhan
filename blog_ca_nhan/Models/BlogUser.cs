using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

[PrimaryKey("BlogId", "UserId")]
public partial class BlogUser
{
    [Key]
    public int BlogId { get; set; }

    [Key]
    public int UserId { get; set; }

    [StringLength(50)]
    public string RoleCode { get; set; } = null!;

    [ForeignKey("BlogId")]
    [InverseProperty("BlogUsers")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("BlogUsers")]
    public virtual User User { get; set; } = null!;
}
