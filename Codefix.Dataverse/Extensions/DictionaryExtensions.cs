using Codefix.Dataverse.Authentication;

namespace Codefix.Dataverse.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static IList<DataverseAuthConfig> GetDataVerseConfigs(this Dictionary<string, DataverseAuthConfig> dict)
        {
            var configs = new List<DataverseAuthConfig>();
            foreach (var val in dict.Values)
            {
                configs.Add(val);
            }

            return configs;
        }

        internal static IList<DataverseAuthConfig> AddRange(this IList<DataverseAuthConfig> currentList, IList<DataverseAuthConfig> listToAdd)
        {
            foreach (var config in listToAdd)
            {
                currentList.Add(config);
            }

            return currentList;
        }
    }
}
