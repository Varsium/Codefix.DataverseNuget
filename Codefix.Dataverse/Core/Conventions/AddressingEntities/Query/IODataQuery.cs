﻿using System;
using System.Collections.Generic;

namespace Codefix.Dataverse.Core.Conventions.AddressingEntities.Query
{
    public interface IODataQuery
    {
        Uri ToUri(UriKind uriKind = UriKind.RelativeOrAbsolute);

        IDictionary<string, string> ToDictionary();

    }
}
