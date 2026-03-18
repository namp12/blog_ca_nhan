using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Post
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    [StringLength(500)]
    public string Title { get; set; } = null!;

    [StringLength(500)]
    public string Slug { get; set; } = null!;

    public string Content { get; set; } = null!;

    [StringLength(1000)]
    public string? Summary { get; set; }

    [StringLength(500)]
    public string? FeaturedImage { get; set; }

    public int? Status { get; set; }

    public bool? IsFeatured { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ViewCount { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("Posts")]
    public virtual User Author { get; set; } = null!;

    [ForeignKey("BlogId")]
    [InverseProperty("Posts")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Posts")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Post")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("Post")]
    public virtual ICollection<PostMetum> PostMeta { get; set; } = new List<PostMetum>();

    [ForeignKey("PostId")]
    [InverseProperty("Posts")]
    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    [ForeignKey("PostId")]
    [InverseProperty("PostsNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
