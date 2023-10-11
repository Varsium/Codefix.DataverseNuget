using System;

namespace Codefix.Dataverse.Core.Conventions.Constants
{
    internal struct QuerySeparators
    {
        public const char Main = '&';

        public const char Nested = ';';

        public const char Begin = '?';

        public const char EqualSign = '=';

        public const char Slash = '/';

        public const char Comma = ',';

        public const char DollarSign = '$';

        public const char RigthBracket = ')';

        public const char LeftBracket = '(';

        [Obsolete("Remove after upgrade to netstandard 2.1")]
        public const string StringComma = ",";

        public const char Dot = '.';
    }
}
