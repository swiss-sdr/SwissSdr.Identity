using FluentValidation;
using Microsoft.Extensions.Localization;
using SwissSdr.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Models
{
    public class RegisterInputModel
	{
		public Gender Gender { get; set; }
		public string Title { get; set; }
		public string Fullname { get; set; }
		public string EMail { get; set; }
		public bool IsTosAccepted { get; set; }
	}
}
