using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace MyLibrary.Json
{
    /// <summary>
    /// 用來完全客製化 json 欄位的類別，可用在 Web API 回傳 Json 的精簡化。
    /// <example>
    /// <para>例如，想讓某個物件序列化成 JSON 之後，只有 name 和 icon 欄位，寫法如下：</para>
    ///     <code>
    ///         <para>var fields new string[] { "name", "icon"};</para>
    ///         <para>JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings{ ContractResolver = new FieldsContractResolver(fields) });</para>
    ///     </code>
    /// </example>
    /// </summary>
    public class FieldsContractResolver : DefaultContractResolver
    {

        private string[] _fields;


        public new static readonly FieldsContractResolver Instance = new FieldsContractResolver(null);
        

        public FieldsContractResolver(string[] fields)
        {
            _fields = fields;
        }


        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            property.ShouldSerialize =
                instance =>
                {
                    return _fields.Contains(property.PropertyName);
                };

            return property;
        }
    }


}


