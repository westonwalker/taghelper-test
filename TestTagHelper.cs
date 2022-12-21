using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;
using System.Reflection.Emit;
using WebApplication3.Views.Components;

namespace WebApplication3
{
	[HtmlTargetElement("*", Attributes = "name")]
	[HtmlTargetElement("*", Attributes = RouteValuesPrefix + "*")]
	[HtmlTargetElement("*", Attributes = RouteValuesDictionaryName + "*")]
	public class TestTagHelper : TagHelper
	{
		[HtmlAttributeName("name")]
		public string Name { get; set; }

		private const string RouteValuesPrefix = "param";
		private const string RouteValuesDictionaryName = "all-param-data";

		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext { get; set; }
		protected IHtmlHelper _html { get; }
		private IDictionary<string, string> _routeValues;

		public TestTagHelper(IHtmlHelper html)
		{
			_html= html;
		}

		[HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
		public IDictionary<string, string> RouteValues
		{
			get
			{
				if (_routeValues == null)
				{
					_routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}

				return _routeValues;
			}
			set
			{
				_routeValues = value;
			}
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			var isComponent = context.TagName.StartsWith("x-");
			if (!isComponent) return;
			var childContent = await output.GetChildContentAsync();
			// var renderedContent = HtmlHelper.Partial("~/Views/Shared/path/to/Template.cshtml");
			(_html as IViewContextAware).Contextualize(ViewContext);
			var test = await _html.RenderComponentAsync<MainButton>(RenderMode.Static, new { Text = "testing" });
			output.Content.SetHtmlContent(test);
			//@(await Html.RenderComponentAsync<DevTeaser>(RenderMode.Static, new { Developers = Model.Developers }))
			// output.Content.SetContent();
			//var html = Generator.
			//output.Content.SetContent();
			//output.TagName = "p";
			//output.Content.SetContent(Name);
			//foreach (var value in _routeValues)
			//{
			//	output.TagName = "p";
			//	output.Content.SetContent(value.Value);
			//}
		}
	}
}
