using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Medium
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int UserId { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [StringLength(50)]
    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    public DateTime? UploadedAt { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("Media")]
    public virtual Blog Blog { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Media")]
    public virtual User User { get; set; } = null!;
}
