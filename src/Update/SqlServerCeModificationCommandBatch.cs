﻿using System;
using System.Data.Common;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.ChangeTracking.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Relational.Metadata;
using Microsoft.Data.Entity.Relational.Update;
using Microsoft.Data.Entity.Update;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Framework.Logging;

namespace ErikEJ.Data.Entity.SqlServerCe.Update
{
    public class SqlServerCeModificationCommandBatch : SingularModificationCommandBatch
    {
        public SqlServerCeModificationCommandBatch([NotNull] ISqlGenerator sqlGenerator)
            : base(sqlGenerator)
        {
        }

        public override IRelationalPropertyExtensions GetPropertyExtensions(IProperty property) => 
            property.Relational();

        public override int Execute(
            RelationalTransaction transaction,
            IRelationalTypeMapper typeMapper,
            DbContext context,
            ILogger logger)
        {
            Check.NotNull(transaction, nameof(transaction));
            Check.NotNull(typeMapper, nameof(typeMapper));
            Check.NotNull(context, nameof(context));
            Check.NotNull(logger, nameof(logger));

            var initialCommandText = GetCommandText();
            Tuple<string, string> commandText = SplitCommandText(initialCommandText);

            Debug.Assert(ResultSetEnds.Count == ModificationCommands.Count);

            var commandIndex = 0;
            using (var storeCommand = CreateStoreCommand(commandText.Item1, transaction.DbTransaction, typeMapper, transaction.Connection?.CommandTimeout))
            {
                if (logger.IsEnabled(LogLevel.Verbose))
                {
                    //TODO Cant log!?
                    //logger.LogCommand(storeCommand);
                }

                try
                {
                    using (var reader = storeCommand.ExecuteReader())
                    {
                        DbCommand returningCommand = null;
                        DbDataReader returningReader = null;
                        try
                        {
                            if (commandText.Item2.Length > 0)
                            {
                                returningCommand = CreateStoreCommand(commandText.Item2, transaction.DbTransaction, typeMapper, transaction.Connection?.CommandTimeout);
                                returningReader = returningCommand.ExecuteReader();
                            }
                            commandIndex = ModificationCommands[commandIndex].RequiresResultPropagation
                            ? ConsumeResultSetWithPropagation(commandIndex, returningReader, context)
                            : ConsumeResultSetWithoutPropagation(commandIndex, reader, context);

                            Debug.Assert(commandIndex == ModificationCommands.Count, "Expected " + ModificationCommands.Count + " results, got " + commandIndex);
                        }
                        finally
                        {
                            if (returningReader != null)
                            {
                                returningReader.Dispose();
                            }
                            if (returningCommand != null)
                            {
                                returningCommand.Dispose();
                            }
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new DbUpdateException(
                        Strings.UpdateStoreException,
                        context,
                        ex);
                }
            }

            return commandIndex;
        }

        private Tuple<string, string> SplitCommandText(string commandText)
        {
            var test = @";
SELECT ";
            string item1 = commandText;
            string item2 = string.Empty;

            if (commandText.Contains(test))
            {
                item1 = commandText.Substring(0, commandText.IndexOf(test) + 1);
                item2 = commandText.Substring(commandText.LastIndexOf(test) + 1);
            }

            return new Tuple<string, string>(item1, item2);
        }

        private int ConsumeResultSetWithoutPropagation(int commandIndex, DbDataReader reader, DbContext context)
        {
            var expectedRowsAffected = 1;
            var rowsAffected = reader.RecordsAffected;
            ++commandIndex;

            if (rowsAffected != expectedRowsAffected)
            {
                throw new DbUpdateConcurrencyException(
                    Strings.UpdateConcurrencyException(expectedRowsAffected, rowsAffected),
                    context);
            }

            return commandIndex;
        }

        //TODO Wait for update EF binaries available from MyGet
        private int ConsumeResultSetWithPropagation(int commandIndex, DbDataReader reader, DbContext context)
        {
            //var rowsAffected = 0;
            //var valueReader = new RelationalTypedValueReader(reader);
            //do
            //{
            //    var tableModification = ModificationCommands[commandIndex];
            //    Debug.Assert(tableModification.RequiresResultPropagation);

            //    if (!reader.Read())
            //    {
            //        var expectedRowsAffected = rowsAffected + 1;
            //        while (++commandIndex < ResultSetEnds.Count
            //               && !ResultSetEnds[commandIndex - 1])
            //        {
            //            expectedRowsAffected++;
            //        }

            //        throw new DbUpdateConcurrencyException(
            //            Strings.UpdateConcurrencyException(expectedRowsAffected, rowsAffected),
            //            context);
            //    }

            //    tableModification.PropagateResults(valueReader);
            //    rowsAffected++;
            //}
            //while (++commandIndex < ResultSetEnds.Count
            //       && !ResultSetEnds[commandIndex - 1]);

            return ++commandIndex;
        }
    }
}
