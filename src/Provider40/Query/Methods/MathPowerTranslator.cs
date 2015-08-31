﻿using System;
using Microsoft.Data.Entity.Query.ExpressionTranslators;

namespace Microsoft.Data.Entity.SqlServerCompact.Query.Methods
{
    public class MathPowerTranslator : SingleOverloadStaticMethodCallTranslator
    {
        public MathPowerTranslator()
            : base(typeof(Math), "Pow", "POWER")
        {
        }
    }
}
