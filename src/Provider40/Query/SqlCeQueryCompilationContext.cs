﻿using JetBrains.Annotations;
using Microsoft.Data.Entity.ChangeTracking.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;
using Microsoft.Data.Entity.Query;
using Microsoft.Data.Entity.Query.Expressions;
using Microsoft.Data.Entity.Query.ExpressionTranslators;
using Microsoft.Data.Entity.Query.Sql;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.SqlServerCompact.Query
{
    public class SqlCeQueryCompilationContext : RelationalQueryCompilationContext
    {
        public SqlCeQueryCompilationContext(
            [NotNull] IModel model,
            [NotNull] ILogger logger,
            [NotNull] ILinqOperatorProvider linqOperatorProvider,
            [NotNull] IResultOperatorHandler resultOperatorHandler,
            [NotNull] IEntityMaterializerSource entityMaterializerSource,
            [NotNull] IEntityKeyFactorySource entityKeyFactorySource,
            [NotNull] IClrAccessorSource<IClrPropertyGetter> clrPropertyGetterSource,
            [NotNull] IQueryMethodProvider queryMethodProvider,
            [NotNull] IMethodCallTranslator compositeMethodCallTranslator,
            [NotNull] IMemberTranslator compositeMemberTranslator,
            [NotNull] IExpressionFragmentTranslator compositeExpressionFragmentTranslator,
            [NotNull] IRelationalValueBufferFactoryFactory valueBufferFactoryFactory,
            [NotNull] IRelationalTypeMapper typeMapper,
            [NotNull] IRelationalMetadataExtensionProvider relationalExtensions)
            : base(
                model,
                logger,
                linqOperatorProvider,
                resultOperatorHandler,
                entityMaterializerSource,
                entityKeyFactorySource,
                clrPropertyGetterSource,
                queryMethodProvider,
                compositeMethodCallTranslator,
                compositeMemberTranslator,
                compositeExpressionFragmentTranslator,
                valueBufferFactoryFactory,
                typeMapper,
                relationalExtensions)
        {
        }

        public override ISqlQueryGenerator CreateSqlQueryGenerator(SelectExpression selectExpression)
            => new SqlCeQuerySqlGenerator(Check.NotNull(selectExpression, nameof(selectExpression)), TypeMapper);
        public override bool IsCrossApplySupported => true;
    }
}
