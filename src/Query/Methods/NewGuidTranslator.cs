﻿using System;
using Microsoft.Data.Entity.Relational.Query.Methods;

namespace Microsoft.Data.Entity.SqlServerCompact.Query.Methods
{
    public class NewGuidTranslator : SingleOverloadStaticMethodCallTranslator
    {
        public NewGuidTranslator()
            : base(typeof(Guid), "NewGuid", "NEWID")
        {
        }
    }
}