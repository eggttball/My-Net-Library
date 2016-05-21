using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;


namespace MyLibrary.Web
{

    /// <summary>
    /// <para>方便在 Route 規則中，排除特定字串</para>
    /// <para>但也可以只用 regular expression，例如：(?!Folder)</para>
    /// </summary>
    public class NotEqualConstraint : IRouteConstraint
    {
        private string[] _matches = null;

        /// <param name="matches">所有想排除的字串</param>
        public NotEqualConstraint(params string[] matches)
        {
            _matches = matches;
        }

        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            bool foundMatch = false;
            foreach (string match in _matches)
                if (String.Compare(values[parameterName].ToString(), match, true) == 0)
                {
                    foundMatch = true;
                    break;
                }

            return !foundMatch;
        }
    }

}
