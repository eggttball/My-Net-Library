using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace System.Web.Mvc.Html
{

    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 輸出一個檔案上傳的表單
        /// </summary>
        public static MvcForm BeginFileForm(this HtmlHelper helper, string actionName = null, object htmlAttributes = null)
        {
            var newHtmlAttributes = new RouteValueDictionary(htmlAttributes);
            if (!newHtmlAttributes.ContainsKey("enctype"))
            {
                // 增加上傳檔案要用的設定
                newHtmlAttributes.Add("enctype", "multipart/form-data");
            }

            return helper.BeginForm(actionName, null, FormMethod.Post, newHtmlAttributes);
        }

        /// <summary>
        /// 合併 HtmlHelper 的 EditorFor 和 ValidationMessageFor，因為這兩者很常一起出現
        /// </summary>
        public static MvcHtmlString EditorValidationFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var str =
                helper.EditorFor(expression).ToString() +
                helper.ValidationMessageFor(expression).ToString();

            return MvcHtmlString.Create(str);
        }


        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            try
            {
                var imgUrl = expression.Compile()(helper.ViewData.Model);
                return BuildImageTag(imgUrl.ToString(), null);
            }
            catch (Exception)
            {
                return BuildImageTag("", null);
            }


        }

        public static MvcHtmlString ImageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var imgUrl = expression.Compile()(helper.ViewData.Model);
            return BuildImageTag(imgUrl.ToString(), htmlAttributes);
        }

        private static MvcHtmlString BuildImageTag(string imgUrl, object htmlAttributes)
        {
            TagBuilder tag = new TagBuilder("img");

            tag.Attributes.Add("src", imgUrl);
            if (htmlAttributes != null)
                tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }


        #region 擴充 HTML.ActionLink


        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(), new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, new RouteValueDictionary(), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, null, routeValues, htmlAttributes, requireAbsoluteUrl);
        }

        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, bool requireAbsoluteUrl)
        {
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new RouteValueDictionary(routeValues), new RouteValueDictionary(htmlAttributes), requireAbsoluteUrl);
        }

        #endregion

    }

}
