﻿using System;
using Microsoft.Data.Entity.Query.ExpressionTranslators;

namespace Microsoft.Data.Entity.SqlServerCompact.Query.Methods
{
    public class MathFloorTranslator : MultipleOverloadStaticMethodCallTranslator
    {
        public MathFloorTranslator()
            : base(typeof(Math), "Floor", "FLOOR")
        {
        }
    }
}
