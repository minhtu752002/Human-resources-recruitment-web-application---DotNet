using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class Applicant
{
    public int ApplicantId { get; set; }

    public string ApplicantName { get; set; } = null!;

    public DateTime? ApplicantDob { get; set; }

    public string? ApplicantPob { get; set; }

    public string? Gender { get; set; }

    public string? ApplicantAddress { get; set; }

    public string? ApplicantPicture { get; set; }

    public string? ApplicantCv { get; set; }

    public int? VacancyId { get; set; }

    public string? ApplicantPhone { get; set; }

    public string? ApplicantEmail { get; set; }

    public string? ActiveEmployee { get; set; }

    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();

    public virtual Vacancy? Vacancy { get; set; }
}
