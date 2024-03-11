using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Fullname { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
}
