using System.Net;

namespace Codefix.Dataverse.Extensions
{
    public static class HttpExtensions
    {
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            var intstate = (int)statusCode;
            return intstate >= 200 && intstate <= 299;
        }
    }
}

