using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Comment
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int PostId { get; set; }

    public int? ParentId { get; set; }

    [StringLength(100)]
    public string AuthorName { get; set; } = null!;

    [StringLength(255)]
    public string AuthorEmail { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsApproved { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("Comments")]
    public virtual Blog Blog { get; set; } = null!;

    [InverseProperty("Parent")]
    public virtual ICollection<Comment> InverseParent { get; set; } = new List<Comment>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Comment? Parent { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("Comments")]
    public virtual Post Post { get; set; } = null!;
}
