using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ResourceMNG.Models
{
    public class ChartViewModel
    {
        public List<string> Vacancies { get; set; }
        public List<int> Applicants { get; set; }
        public List<int> InterviewResults { get; set; }
    }
}
