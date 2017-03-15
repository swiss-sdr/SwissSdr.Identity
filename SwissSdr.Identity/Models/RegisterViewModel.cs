using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.Extensions.Localization;
using SwissSdr.Datamodel;
using SwissSdr.Identity.Resources;
using System.ComponentModel;

namespace SwissSdr.Identity.Models
{
	public class RegisterViewModel : RegisterInputModel
	{
		public string ReturnUrl { get; set; }
	}

	public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
	{
		public RegisterViewModelValidator(IStringLocalizer<RegisterViewModel> localizer)
		{
			RuleFor(x => x.Fullname)
				.NotEmpty()
				.WithLocalizedMessage(localizer.FullnameValidationMessage());
			RuleFor(x => x.EMail)
				.NotEmpty()
				.WithLocalizedMessage(localizer.EmailValidationMessage());
			RuleFor(x => x.IsTosAccepted)
				.Must(accept => accept)
				.WithLocalizedMessage(localizer.TosAcceptValidationMessage());
		}
	}
}
