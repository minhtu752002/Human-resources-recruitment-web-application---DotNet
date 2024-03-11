using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResourceEntity.Models
{
	public class NewApplicant : Applicant
	{
		
		public IFormFile? FileApplicantCv { set; get; }

		public IFormFile? FileApplicantPicture { set; get; }
	}
}
