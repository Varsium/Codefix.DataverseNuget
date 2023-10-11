namespace Codefix.Dataverse.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public sealed class ODataTableAttribute : Attribute
    {
        public string? Name { get; set; }

        public ODataTableAttribute(string name)
        {
            Name = name;
        }
    }
}
