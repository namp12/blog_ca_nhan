using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class PostMetum
{
    [Key]
    public int Id { get; set; }

    public int PostId { get; set; }

    [StringLength(100)]
    public string? MetaKey { get; set; }

    public string? MetaValue { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("PostMeta")]
    public virtual Post Post { get; set; } = null!;
}
