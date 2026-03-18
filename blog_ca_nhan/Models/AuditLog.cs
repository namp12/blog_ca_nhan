using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class AuditLog
{
    [Key]
    public int Id { get; set; }

    public int? BlogId { get; set; }

    public int? UserId { get; set; }

    [StringLength(100)]
    public string? Action { get; set; }

    public string? Details { get; set; }

    public DateTime? CreatedAt { get; set; }

    [Column("IPAddress")]
    [StringLength(50)]
    public string? Ipaddress { get; set; }

    [ForeignKey("BlogId")]
    [InverseProperty("AuditLogs")]
    public virtual Blog? Blog { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User? User { get; set; }
}
