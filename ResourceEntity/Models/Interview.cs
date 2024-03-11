using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class Interview
{
    public int InterviewId { get; set; }

    public DateTime? InterviewSd { get; set; }

    public DateTime? InterviewEd { get; set; }

    public string? InterviewDescription { get; set; }

    public string? InterviewAddress { get; set; }

    public int? ApplicantId { get; set; }

    public string? InterviewResultOne { get; set; }

    public string? InterviewResultTwo { get; set; }

    public string? InterviewSchedule { get; set; }

    public virtual Applicant? Applicant { get; set; }
}
