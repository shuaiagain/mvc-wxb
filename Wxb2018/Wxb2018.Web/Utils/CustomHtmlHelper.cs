using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Wxb2018.Web.Utils
{
    public static class CustomHtmlHelper
    {

        public static IHtmlString ValidationMsgFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string message, object htmlAttributes)
        {
            
            if (htmlAttributes != null)
            {
                IDictionary<string, object> dic = GetDictionary(htmlAttributes);
                return ValidationMsgFor(helper, expression, message, dic);
            }
            return ValidationMsgFor(helper, expression, message);
        }
        public static IHtmlString ValidationMsgFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string message = null, IDictionary<string, object> htmlAttributes = null)
        {
            string propertyName = ExpressionHelper.GetExpressionText(expression);
            string name = helper.AttributeEncode(helper.ViewData.TemplateInfo.GetFullHtmlFieldName(propertyName));
            return ValidationMsg(helper, name, message, htmlAttributes);
        }
        public static IHtmlString ValidationMsg(this HtmlHelper helper, string name, string message, object htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                IDictionary<string, object> dic = GetDictionary(htmlAttributes);
                return ValidationMsg(helper, name, message, dic);
            }
            return ValidationMsg(helper, name, message, null);

        }

        public static IHtmlString ValidationMsg(this HtmlHelper helper, string name, string message = null, IDictionary<string, object> htmlAttributes = null)
        {
            if (helper.ViewData.ModelState.IsValidField(name))
            {
                return null;
            }
            if (string.IsNullOrEmpty(message))
            {
                var errors = helper.ViewData.ModelState[name].Errors;
                var error = errors.FirstOrDefault();
                if (error != null)
                {
                    message = error.ErrorMessage;
                }
            }
            if (message == null)
            {
                return null;
            }
            TagBuilder tag = new TagBuilder("div") { InnerHtml = message };
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("class", "field-validation-error");
            tag.Attributes.Add("data-valmsg-for", name);
            tag.Attributes.Add("data-valmsg-replace", "true");
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static IDictionary<string, object> GetDictionary(object htmlAttributes)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            htmlAttributes.GetType().GetProperties().ToList().ForEach(o =>
            {
                dic.Add(o.Name, o.GetValue(htmlAttributes, null));
            });
            return dic;
        }
        //public static string NameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        //{
        //    return html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
        //}

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="wrapperTagName"></param>
        /// <param name="currentPage"></param>
        /// <param name="wrapperTagHtmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString Pagination(this HtmlHelper helper, int totalCount, string wrapperTagName, int pageSize, int currentPage = 1, object wrapperTagHtmlAttributes = null, string link = null, string ellipseText=null)
        {
            var pagination = new Pagination();
            pagination.TotelCount=totalCount;
            pagination.PageSize = pageSize;
            pagination.Current = currentPage;
            pagination.Link =link??string.Empty;
            pagination.CurrentPageClass = "on";
            pagination.EllipseText = ellipseText ?? "<span>...</span>";
            pagination.Init();
            var script = "<script> if ($('.pageGro > a:nth-child(2)').text()!='1') { $('<span>...</span>').insertAfter($('.pageGro a:contains(上一页)')); } </script>";
            TagBuilder tag = new TagBuilder(wrapperTagName) { InnerHtml = pagination.Render() +script};
            if (wrapperTagHtmlAttributes != null)
            {
                IDictionary<string, object> dic = GetDictionary(wrapperTagHtmlAttributes);
                tag.MergeAttributes(dic);
            }
            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
         }

        //public static string ContentV(this UrlHelper helper, string path)
        //{
        //    if (!Config.IsProd)
        //    {
        //        var v = "";
        //        try
        //        {
        //            var filePath = HttpContext.Current.Server.MapPath(path);
        //            if (File.Exists(filePath))
        //            {
        //                v = System.IO.File.GetLastAccessTime(filePath).Ticks.ToString();
        //            }
        //        }
        //        catch { }
        //        return helper.Content(path + "?v=" + v);
        //    }
        //    if (string.IsNullOrWhiteSpace(path))
        //    {
        //        return "";
        //    }
        //    path = path.Trim();
        //    if (path[0] == '~')
        //    {
        //        path = path.Substring(1);
        //    }
        //    return Config.StaticFileBaseUrl + path + "?ver=" + (CDNVersion.Instance[path]);

        //}

        #region 正式环境 需要分离pay server
        /// <summary>
        /// 正式环境 需要分离pay server
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        //public static string ActionF(this UrlHelper helper, string actionName, string controllerName)
        //{

        //    var url = helper.Action(actionName, controllerName);
        //    return UrlFormat(url, controllerName);

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        //public static string ActionF(this UrlHelper helper, string actionName, string controllerName,object values)
        //{

        //    var url = helper.Action(actionName, controllerName,values);
        //    var baseUrl = helper.Content("~/");
        //    return UrlFormat(url.Substring(baseUrl.Length-1),controllerName);
        //}
        
        #endregion

        #region  正式机pay url与主站url不一样,需单独生成url
        /// <summary>
        /// 正式机pay url与主站url不一样,需单独生成url
        /// </summary>
        /// <param name="actionUrl"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        //private static string UrlFormat(string actionUrl, string controllerName)
        //{

        //    var url = actionUrl;

        //    if (controllerName.ToLower() == "pay")
        //    {

        //            url = Config.DomainPayUrl.TrimEnd('/') + url;

        //    }
        //    else
        //    {
        //        url = Config.DomainUrl.TrimEnd('/') + url;
        //    }
        //    return url;
        //} 
        #endregion
    }

}