using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SwissSdr.Identity.Infrastructure
{
	[HtmlTargetElement(Attributes = AttributeName)]
	public class ValidationStateTagHelper : TagHelper
	{
		public const string AttributeName = "validationstate-for";

		[HtmlAttributeName(AttributeName)]
		public ModelExpression For { get; set; }

		[ViewContext]
		[HtmlAttributeNotBound]
		public ViewContext ViewContext { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.Attributes.RemoveAll(AttributeName);
			if (ViewContext.ModelState.GetValidationState(For.Name) == ModelValidationState.Invalid)
			{
				var existingClasses = output.Attributes.SingleOrDefault(a => a.Name == "class")?.Value.ToString().Split(' ')
					?? Enumerable.Empty<string>();

				output.Attributes.SetAttribute("class", string.Join(" ", existingClasses.Concat(new[] { "is-invalid" })));
			}
		}
	}
}
