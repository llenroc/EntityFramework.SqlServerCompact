﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Query.ExpressionTranslators;
using Microsoft.Data.Entity.SqlServerCompact.Query.Methods;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.SqlServerCompact.Query.ExpressionTranslators
{
    public class SqlCeCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        public SqlCeCompositeMethodCallTranslator([NotNull] ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            var sqlCeTranslators = new List<IMethodCallTranslator>
            {
                new NewGuidTranslator(),
                new StringSubstringTranslator(),
                new MathAbsTranslator(),
                new MathCeilingTranslator(),
                new MathFloorTranslator(),
                new MathPowerTranslator(),
                new MathRoundTranslator(),
                new MathTruncateTranslator(),
                new StringReplaceTranslator(),
                new StringToLowerTranslator(),
                new StringToUpperTranslator(),
                new ConvertTranslator(),
            };

            AddTranslators(sqlCeTranslators);
        }
    }
}
