using RestSharp;

namespace Launcher.Models
{
    public class CamelCaseJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            return char.ToLower(clrPropertyName[0]) + clrPropertyName.Substring(1);
        }
    }
}