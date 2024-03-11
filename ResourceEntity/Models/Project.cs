using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string? ProjectName { get; set; }

    public string? ProjectDescription { get; set; }

    public string? ProjectClient { get; set; }

    public string? ProjectActive { get; set; }

    public string? ProjectDelete { get; set; }
}
