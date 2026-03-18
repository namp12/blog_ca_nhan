using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class Subscriber
{
    [Key]
    public int Id { get; set; }

    public int BlogId { get; set; }

    [StringLength(255)]
    public string Email { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("Subscribers")]
    public virtual Blog Blog { get; set; } = null!;
}
