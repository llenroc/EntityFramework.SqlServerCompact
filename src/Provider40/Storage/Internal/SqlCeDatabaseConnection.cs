﻿using System.Data.Common;
using System.Data.SqlServerCe;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Framework.Logging;

namespace Microsoft.Data.Entity.Storage.Internal
{
    public class SqlCeDatabaseConnection : RelationalConnection, ISqlCeDatabaseConnection
    { 
        public SqlCeDatabaseConnection([NotNull] IDbContextOptions options, [NotNull] ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }

        protected override DbConnection CreateDbConnection() => new SqlCeConnection(ConnectionString);

        public override bool IsMultipleActiveResultSetsEnabled => true;
    }
}