using System.Text.Json.Serialization;

namespace Codefix.Dataverse.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    internal class OdataNameAttribute : JsonAttribute
    {
        public OdataNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}
