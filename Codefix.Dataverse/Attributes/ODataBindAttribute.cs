using Codefix.Dataverse.Extensions;

namespace Codefix.Dataverse.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ODataBindAttribute : Attribute
    {
        public ODataBindAttribute(string name, string referenceTable)
        {
            Name = name;
            ReferenceTable = referenceTable;
        }
        public ODataBindAttribute(string name, Type type)
        {
            Name = name;
            ReferenceTable = type.GetTableName();
        }

        public string Name { get; set; }
        public string ReferenceTable { get; set; }

    }
}
