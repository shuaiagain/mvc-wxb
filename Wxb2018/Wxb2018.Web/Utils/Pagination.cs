using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wxb2018.Web.Utils
{
    public class Pagination
    {
        #region 属性

        /// <summary>
        /// 上一页的文本
        /// </summary>
        public string PreText { get; set; }
        /// <summary>
        /// 下一页的文本
        /// </summary>
        public string NextText { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotelCount { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页的样式,默认 class="current"
        /// </summary>
        public string  CurrentPageClass { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int Current
        {
            get
            {
                if (pageNum == -1)
                {
                    string page = HttpContext.Current.Request.QueryString["page"] ?? "1";
                    if (!int.TryParse(page, out pageNum))
                    {
                        pageNum = 1;
                    }
                }
                return pageNum;
            }
            set
            {
                pageNum = value;
            }
        }

        /// <summary>
        /// 中间显示的链接数，默认为10
        /// </summary>
        public int DisplayCount { get; set; }

        /// <summary>
        /// 链接网址
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 省略的页数用什么文字表示，默认是"…"
        /// </summary>
        public string EllipseText { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }

        #endregion

        public string Render()
        {
            var sb = new System.Text.StringBuilder();
            if (Current - 1 <= 0)
            {
                sb.AppendFormat("<a  class='pageUp bgbcbcbc' href='javascript:void()'>{0}</a>", PreText);
            }
            else
            {
                sb.AppendFormat("<a target='_self' class='pageUp'  href='{0}' >{1}</a>", LinkFormat(Current - 1), PreText);
            }
            sb.AppendFormat(AElementsHtml());

            if (Current + 1 > PageCount)
            {
                sb.AppendFormat("<a class='pageDown bgbcbcbc' href='javascript:void()'>{0}</a>", NextText);
            }
            else
            {
                sb.AppendFormat("<a target='_self' class='pageDown' href='{0}' >{1}</a>", LinkFormat(Current + 1), NextText);
            }
            return sb.ToString();
        }

        public void Init()
        {
            //赋值--Page_Load--页面计算
            DisplayCount = DisplayCount <= 0 ? 5 : DisplayCount;
            TotelCount = TotelCount <= 0 ? 1 : TotelCount;
            PageSize = PageSize <= 0 ? 1 : PageSize;
            PageCount = (int)Math.Ceiling(1.0 * TotelCount / PageSize);
            EllipseText = string.IsNullOrWhiteSpace(EllipseText) ? "..." : EllipseText;
            PreText = NextText == null ? "上一页" : PreText;
            NextText = NextText == null ? "下一页" : NextText;
            Current = Current > PageCount ? PageCount : Current;
            #region Link
            Link = Link ?? HttpContext.Current.Request.Url.ToString();
            Link += Link.Contains('?') ? "" : "?";
            Link += string.IsNullOrWhiteSpace(Link.Split('?')[1]) ? "" : "&";
            #endregion
        }


        protected string LinkFormat(int page)
        {
            // var list = Link.Split('_').ToList();
            //list.Insert(1, page.ToString());
            // return string.Join("_", list.ToArray());
            return string.Format("{0}page={1}", Link, page);
        }

        protected string AElementsHtml()
        {
            List<string> aList = new List<string>();
            if (DisplayCount >= PageCount)
            {
                for (int i = 1; i <= PageCount; i++)
                {
                    aList.Add(AElement(i));
                }
            }
            else if (DisplayCount < 8)
            {
                if (Current < DisplayCount)
                {
                    for (int i = 1; i <= DisplayCount; i++)
                    {
                        aList.Add(AElement(i));
                    }
                    aList.Add(EllipseText);
                }
                else
                {
                    int leftCount = DisplayCount / 2;
                    int rigthCont = DisplayCount - leftCount - 1;
                    int total = Current + rigthCont;
                    total = total > PageCount ? PageCount : total;
                    for (int i = Current - leftCount; i <= total; i++)
                    {
                        aList.Add(AElement(i));
                    }
                    if (total < PageCount)
                    {
                        aList.Add(EllipseText);
                    }

                }
            }
            else if (IsBetween(Current, 1, DisplayCount - 3))
            {
                for (int i = 1; i <= DisplayCount - 2; i++)
                {
                    aList.Add(AElement(i));
                }
                aList.Add(EllipseText);
                aList.Add(AElement(PageCount - 1));
                aList.Add(AElement(PageCount));
            }
            else if (IsBetween(Current, PageCount - (DisplayCount - 4), PageCount))
            {
                aList.Add(AElement(1));
                aList.Add(AElement(2));
                aList.Add(EllipseText);
                for (int i = PageCount - (DisplayCount - 3); i <= PageCount; i++)
                {
                    aList.Add(AElement(i));
                }
            }
            else
            {
                aList.Add(AElement(1));
                aList.Add(AElement(2));
                aList.Add(EllipseText);
                int remainCount = DisplayCount - 5;
                int leftCount = remainCount / 2;
                int rigthCont = remainCount - leftCount;
                for (int i = Current - leftCount; i <= Current + rigthCont; i++)
                {
                    aList.Add(AElement(i));
                }
                aList.Add(EllipseText);
                aList.Add(AElement(PageCount - 1));
                aList.Add(AElement(PageCount));
            }
            return string.Join(" ", aList);
        }

        /// <summary>
        /// A 元素
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private string AElement(int page)
        {
            if (page == Current)
            {
                return string.Format(@"<a href='javascript:void()' class='{1}'>{0}</a>", page, CurrentPageClass ?? "current");
            }
            else
            {
                return string.Format(@"<a href='{0}'>{1}</a>", LinkFormat(page), page);
            }
        }

        private bool IsBetween(int num, int begin, int end)
        {
            return begin <= num && num <= end;
        }
        private int pageNum = -1;
    }


}