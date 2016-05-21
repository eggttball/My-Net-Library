using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.System
{

    public static class EnumExtensions
    {
        /// <summary>
        /// 取得 Enum 的字串值（該值應藉由 <see cref="MyLibrary.System.StringValueAttribute"/> 指定）
        /// </summary>
        public static string StrValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo info = type.GetField(value.ToString());
            StringValueAttribute[] attribs = info.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attribs.Length > 0 ? attribs[0].Value : null;
        }


    }

}


