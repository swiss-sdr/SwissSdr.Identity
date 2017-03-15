using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using SwissSdr.Datamodel;
using SwissSdr.Identity.Controllers;
using SwissSdr.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Resources
{
	public static class RegisterViewModelExtensions
	{
		public static string Title(this IStringLocalizer<RegisterViewModel> localizer) => localizer["Title"];

		public static string GenderLabel(this IStringLocalizer<RegisterViewModel> localizer, Gender gender) => localizer[$"GenderLabel_{gender}"];

		public static string TitleLabel(this IStringLocalizer<RegisterViewModel> localizer) => localizer["TitleLabel"];
		public static string FullnameLabel(this IStringLocalizer<RegisterViewModel> localizer) => localizer["FullnameLabel"];
		public static string EmailLabel(this IStringLocalizer<RegisterViewModel> localizer) => localizer["EmailLabel"];
		public static LocalizedHtmlString TosAcceptLabel(this IHtmlLocalizer<RegisterViewModel> localizer, params object[] arguments) => localizer["TosAcceptLabel", arguments];

		public static string FullnameValidationMessage(this IStringLocalizer<RegisterViewModel> localizer) => localizer["FullnameValidationMessage"];
		public static string EmailValidationMessage(this IStringLocalizer<RegisterViewModel> localizer) => localizer["EmailValidationMessage"];
		public static string TosAcceptValidationMessage(this IStringLocalizer<RegisterViewModel> localizer) => localizer["TosAcceptValidationMessage"];

		public static string CreateAccountAction(this IStringLocalizer<RegisterViewModel> localizer) => localizer["CreateAccountAction"];
	}

	public static class LoggedOutViewModelExtensions
	{
		public static string Title(this IStringLocalizer<LoggedOutViewModel> localizer) => localizer["Title"];
		public static string Action(this IStringLocalizer<LoggedOutViewModel> localizer) => localizer["Action"];
		public static string TextNoRedirect(this IStringLocalizer<LoggedOutViewModel> localizer) => localizer["TextNoRedirect"];
		public static string TextWithPostLogoutRedirect(this IStringLocalizer<LoggedOutViewModel> localizer) => localizer["TextWithPostLogoutRedirect"];
	}

	public static class LogoutViewModelExtensions
	{
		public static string Title(this IStringLocalizer<LogoutViewModel> localizer) => localizer["Title"];
		public static string Action(this IStringLocalizer<LogoutViewModel> localizer) => localizer["Action"];
		public static string Text(this IStringLocalizer<LogoutViewModel> localizer) => localizer["Text"];
	}

	public static class RemoveExternalLoginViewModelExtensions
	{
		public static string Title(this IStringLocalizer<RemoveExternalLoginViewModel> localizer) => localizer["Title"];
		public static string ActionConfirm(this IStringLocalizer<RemoveExternalLoginViewModel> localizer) => localizer["ActionConfirm"];
		public static string ActionCancel(this IStringLocalizer<RemoveExternalLoginViewModel> localizer) => localizer["ActionCancel"];
		public static string Text(this IStringLocalizer<RemoveExternalLoginViewModel> localizer, string provider) => string.Format(localizer["Text"], provider);
	}

	public static class HomeControllerExtensions
	{
		public static string AppUrl(this IStringLocalizer<HomeController> localizer) => localizer["AppUrl"];
		public static string OpenAppAction(this IStringLocalizer<HomeController> localizer) => localizer["OpenAppAction"];
	}
}
