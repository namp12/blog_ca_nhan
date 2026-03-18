using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace blog_ca_nhan.Models;

public partial class PlatformSetting
{
    [Key]
    [StringLength(100)]
    public string Key { get; set; } = null!;

    public string? Value { get; set; }

    [StringLength(50)]
    public string? GroupCode { get; set; }
}
