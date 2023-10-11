using CrossBorder.Dataverse.Core;

namespace Sandbox
{
    public class SandboxDb: ODataDbContext
    {
        public ODataDbSet<Person> Persons { get; set; }
        public ODataDbSet<Measure> Measures { get; set; }
    }
}
