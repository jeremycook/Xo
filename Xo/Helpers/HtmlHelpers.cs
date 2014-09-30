using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Xo.Helpers
{
    public static class HtmlHelpers
    {
        public static IHtmlString BootstrapLabelFor<TModel, TProp>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProp>> expression,
            string columnClass = "col-md-2")
        {
            return html.LabelFor(expression, htmlAttributes: new { @class = columnClass + " control-label" });
        }

        public static IHtmlString BootstrapLabel(
            this HtmlHelper html,
            string propertyName,
            string columnClass = "col-md-2")
        {
            return html.Label(propertyName, htmlAttributes: new { @class = columnClass + " control-label" });
        }
    }
}