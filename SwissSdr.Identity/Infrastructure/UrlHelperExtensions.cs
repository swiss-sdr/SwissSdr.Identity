using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwissSdr.Identity
{
	public static class UrlHelperExtensions
	{
		/// <summary>
		/// Generates an absolute URL using the specified route name and <see cref="Expression{TDelegate}"/> for an action method,
		/// from which action name, controller name and route values are resolved.
		/// </summary>
		/// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
		/// <param name="action">
		/// The <see cref="Expression{TDelegate}"/>, from which action name, 
		/// controller name and route values are resolved.
		/// </param>
		/// <returns>The generated absolute URL.</returns>
		public static string Link<TController>(this IUrlHelper helper, Expression<Action<TController>> action) where TController : class
		{
			return helper.Link(action, null);
		}

		/// <summary>
		/// Generates an absolute URL using the specified route name, <see cref="Expression{TDelegate}"/> for an action method,
		/// from which action name, controller name and route values are resolved, and the specified additional route values.
		/// </summary>
		/// <typeparam name="TController">Controller, from which the action is specified.</typeparam>
		/// <param name="action">
		/// The <see cref="Expression{TDelegate}"/>, from which action name, 
		/// controller name and route values are resolved.
		/// </param>
		/// <param name="values">An object that contains additional route values.</param>
		/// <returns>The generated absolute URL.</returns>
		public static string Link<TController>(this IUrlHelper helper, Expression<Action<TController>> action, object values) where TController : class
		{
			var request = helper.ActionContext.HttpContext.Request;

			return helper.Action(action, values, request.Scheme, request.Host.ToString());
		}
	}
}
