using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codefix.Dataverse.Helpers
{
    public static class ODataHelper
    {
        public const string EXPAND = "$expand";
        public const string FILTER = "$filter";
        public const string ORDERBY = "$orderby";
        public const string SELECT = "$select";
        public const string TOP = "$top";
        public const string SKIP = "$skip";
        public const string COUNT = "$count=true";
        public const string SEARCH = "$search";

        private readonly static Dictionary<string, string> _expressionsToOdata = new Dictionary<string, string>() {
                { "OrElse", "or" },
                { "AndAlso", "and" },
                { "==", "eq" },
                { "Not", "ne" },
                { "!=", "ne" },
                { ">", "gt" },
                { ">=", "ge" },
                { "<", "lt" },
                { "<=", "le" },
                { "||", "or" },
                { "Equal","eq" },
            {"NotEqual","ne" }
            };


        public static string ReplaceExpressionWithODataQuery(this ExpressionType type)
        {
            if (_expressionsToOdata.ContainsKey(type.ToString()))
            {
                return _expressionsToOdata[type.ToString()];
            }
            throw new Exception("Conversion from expression to OdataQuery failed");
        }

    }
}
