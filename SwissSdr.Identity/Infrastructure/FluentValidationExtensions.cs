using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity
{
    public static class FluentValidationExtensions
    {
		public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string localizedMessage)
		{
			return rule.WithMessage("{0}", localizedMessage);
		}
	}
}
