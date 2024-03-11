using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEntity.Models
{
    public class NewEmployee : Employee
    {
        public IFormFile? FileEmployeeCv { set; get; }

        public IFormFile? FileEmployeeAvatar { set; get; }
    }
}
