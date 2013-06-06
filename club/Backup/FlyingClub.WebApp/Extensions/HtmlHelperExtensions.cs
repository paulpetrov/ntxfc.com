using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Text;
using System.Web.Routing;


namespace FlyingClub.WebApp.Extensions
{
    public static class CheckboxListHelper
    {
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, 1);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList, 
            int numberOfColumns)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, numberOfColumns);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes, int numberOfColumns)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), numberOfColumns);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes, 
            int numberOfColumns)
        {
            string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            // Get the property (and assume IEnumerable)
            IEnumerable currentValues = (IEnumerable)expression.Compile().Invoke(htmlHelper.ViewData.Model);
            int columnCount = 0;
            StringBuilder sb = new StringBuilder();

            foreach (var option in selectList)
            {
                columnCount++;
                TagBuilder builder = new TagBuilder("input");
                if (null != currentValues)
                {
                    var enumerator = currentValues.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.ToString() == option.Value)
                        {
                            builder.MergeAttribute("checked", "checked");
                            break;
                        }
                    }
                }

                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", option.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = option.Text;
                sb.Append(builder.ToString(TagRenderMode.Normal));

                if (columnCount == numberOfColumns)
                {
                    columnCount = 0;
                    sb.Append("<br />");
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

    }

    public static class ViewHelper
    {
        public static MvcHtmlString PageTitle(this HtmlHelper helper, PageTitleImage image, string pageType, string pageDescription)
        {
            string imageUrl = string.Empty;

            switch (image)
            {
                case PageTitleImage.Aircraft:
                    imageUrl = "~/Content/themes/base/images/paper_plane.png";
                    break;
                case PageTitleImage.Member:
                    imageUrl = "~/Content/themes/base/images/person.png";
                    break;
                case PageTitleImage.Squawk:
                    imageUrl = "~/Content/themes/base/images/wrench.png";
                    break;
                case PageTitleImage.Reservation:
                    imageUrl = "~/Content/themes/base/images/flight_plan.gif";
                    break;
            }

            String result = String.Format(
                "<img src=\"{0}\" align=\"left\" alt=\"{1}\" class=\"pageTitleIcon\" /><h1 class=\"pageType\">{1}</h1><h2 class=\"pageDescription\">{2}</h2><div style=\"clear: both;\"></div>"
                , UrlHelper.GenerateContentUrl(imageUrl, helper.ViewContext.HttpContext)
                , pageType
                , pageDescription);
            
            return  MvcHtmlString.Create(result);
        }
    }

    public enum PageTitleImage
    {
        Squawk,
        Aircraft,
        Member,
        Reservation
    }
}