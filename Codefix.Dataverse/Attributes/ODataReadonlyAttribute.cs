namespace Codefix.Dataverse.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ODataReadonlyAttribute : Attribute
    {
        public ODataReadonlyAttribute()
        {
            Readonly = true;
        }

        public ODataReadonlyAttribute(bool _readonly)
        {
            Readonly = _readonly;
        }

        public bool Readonly { get; set; }
    }
}
