using System.Text;

namespace Codefix.Dataverse.Extensions
{
    public static class StringExtensions
    {

        public static string DecodeBase64_UTF(this string x)
        {
            try
            {

                byte[] decodedBytes = Convert.FromBase64String(x);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (Exception)
            {
                try
                {
                    x += "=";
                    byte[] decodedBytes = Convert.FromBase64String(x);
                    return Encoding.UTF8.GetString(decodedBytes);
                }
                catch (Exception)
                {
                    x.Remove(x.LastIndexOf("="), 1);
                    return x;
                }
            }
        }

        public static Guid ToGuid(this string x)
        {
            if (Guid.TryParse(x, out var result))
            {
                return result;
            }

            return Guid.Empty;
        }
        public static bool IsAsPatch(this string x)
        {
            if (x.Length < 3)
            {
                return false;
            }
            var endGuid = x.IndexOf(')');
            var startGuid = x.IndexOf('(');
            if (endGuid - startGuid > 1)
            {
                return true;
            }
            return false;
        }
    }
}
