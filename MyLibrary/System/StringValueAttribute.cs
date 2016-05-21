using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.System
{

    /// <summary>
    /// 可賦予一個欄位額外的字串值，通常用於指定 Enum 的字串值（因為 Enum 本身只能儲存數字）
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class StringValueAttribute : Attribute
    {
        private readonly string _value;

        public string Value { get { return _value; } }

        public StringValueAttribute(string value)
        {
            this._value = value;
        }

    }

}


