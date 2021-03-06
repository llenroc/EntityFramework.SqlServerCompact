﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class SqlCeCompositeMemberTranslator : RelationalCompositeMemberTranslator
    {
        public SqlCeCompositeMemberTranslator([NotNull] RelationalCompositeMemberTranslatorDependencies dependencies)
            : base(dependencies)
        {
            var sqlCeTranslators = new List<IMemberTranslator>
            {
                new SqlCeStringLengthTranslator(),
                new SqlCeDateTimeNowTranslator(),
                new SqlCeDateTimeDatePartComponentTranslator(),
            };

            // ReSharper disable once VirtualMemberCallInContructor
            AddTranslators(sqlCeTranslators);
        }
    }
}
