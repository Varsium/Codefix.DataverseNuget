namespace Codefix.Dataverse.Enums
{
    //public enum JusticeProject
    //{
    //    JustSend,
    //    Ter67,
    //    Contestations,
    //    CrossPortal,
    //    JustRequest,
    //    JustSignal,
    //    DataBroker,
    //    PVV,
    //    ThreeSixty,
    //}
    public abstract class JusticeProject
    {
        public static string JustSend { get; set; } = nameof(JustSend);
        public static string Ter67 { get; set; } = nameof(Ter67);
        public static string Contestations { get; set; } = nameof(Contestations);
        public static string CrossPortal { get; set; } = nameof(CrossPortal);
        public static string JustRequest { get; set; } = nameof(JustRequest);
        public static string JustSignal { get; set; } = nameof(JustSignal);
        public static string DataBroker { get; set; } = nameof(DataBroker);
        public static string PVV { get; set; } = nameof(PVV);
        public static string ThreeSixty { get; set; } = nameof(ThreeSixty);

        //new From version 1.1.19
        public static string Ifc { get; set; } = nameof(Ifc);
        public static string JustId { get; set; } = nameof(JustId);
        public static string JustTemplate { get; set; } = nameof(JustTemplate);
    }
}
