using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codefix.Dataverse.Core
{
    internal enum EntityState
    {
        Created = 0,
        Synced = 1,
        Updated = 2,
        Deleted = 3,

    }
}
