using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class Vacancy
{
    public int VacancyId { get; set; }

    public string? VacancyJobTitle { get; set; }

    public string? VacancyDetails { get; set; }

    public DateTime? VacancyOd { get; set; }

    public DateTime? VacancyCd { get; set; }

    public string? VacancyHm { get; set; }

    public int? VacancyAmout { get; set; }

    public virtual ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
}
