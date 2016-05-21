using System;
using System.Web.WebPages;


namespace MyLibrary.Web
{

    public static class SectionExtensions
    {
        private static readonly object _o = new object();

        /// <summary>
        /// 讓前端 RenderSection 可以同時加上預設內容(如果找不到該 Section)
        /// </summary>
        public static HelperResult RenderSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
                return page.RenderSection(sectionName);
            else
                return defaultContent(_o);
        }


        public static HelperResult RedefineSection(this WebPageBase page, string sectionName)
        {
            return RedefineSection(page, sectionName, defaultContent: null);
        }


        public static HelperResult RedefineSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
                page.DefineSection(sectionName, () => page.Write(page.RenderSection(sectionName)));

            else if (defaultContent != null)
                page.DefineSection(sectionName, () => page.Write(defaultContent(_o)));

            return new HelperResult(_ => { });
        }
    }

}
