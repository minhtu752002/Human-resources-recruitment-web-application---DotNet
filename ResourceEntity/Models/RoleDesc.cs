using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class RoleDesc
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public string? Link { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? Title { get; set; }

    public int? Stt { get; set; }

    public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
}
