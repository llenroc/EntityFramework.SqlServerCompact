﻿using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    public static class SqlCeInternalMetadataBuilderExtensions
    {
        public static RelationalModelBuilderAnnotations SqlCe(
            [NotNull] this InternalModelBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalModelBuilderAnnotations(builder, configurationSource);

        public static RelationalPropertyBuilderAnnotations SqlCe(
            [NotNull] this InternalPropertyBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalPropertyBuilderAnnotations(builder, configurationSource);

        public static RelationalEntityTypeBuilderAnnotations SqlCe(
            [NotNull] this InternalEntityTypeBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalEntityTypeBuilderAnnotations(builder, configurationSource);

        public static RelationalKeyBuilderAnnotations SqlCe(
            [NotNull] this InternalKeyBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalKeyBuilderAnnotations(builder, configurationSource);

        public static RelationalIndexBuilderAnnotations SqlCe(
            [NotNull] this InternalIndexBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalIndexBuilderAnnotations(builder, configurationSource);

        public static RelationalForeignKeyBuilderAnnotations SqlCe(
            [NotNull] this InternalRelationshipBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalForeignKeyBuilderAnnotations(builder, configurationSource);
    }
}
