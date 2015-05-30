﻿using System;
using System.Collections.Generic;
using ErikEJ.Data.Entity.SqlServerCe.Metadata;
using ErikEJ.Data.Entity.SqlServerCe.Migrations;
using ErikEJ.Data.Entity.SqlServerCe.Update;
using ErikEJ.Data.Entity.SqlServerCe.ValueGeneration;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.History;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using Microsoft.Data.Entity.Relational.Migrations.Sql;
using Microsoft.Data.Entity.Relational.Update;
using Microsoft.Data.Entity.Tests;
using Microsoft.Framework.DependencyInjection;
using Xunit;

namespace ErikEJ.Data.Entity.SqlServerCe.Tests.Extensions
{
    public class SqlCeEntityFrameworkServicesBuilderExtensionsTest : EntityFrameworkServiceCollectionExtensionsTest
    {
        [Fact]
        public override void Services_wire_up_correctly()
        {
            base.Services_wire_up_correctly();

            // Relational
            VerifySingleton<IComparer<ModificationCommand>>();

            // SQL Server Ce dingletones
            VerifySingleton<SqlCeModelBuilderFactory>();
            VerifySingleton<SqlCeValueGeneratorCache>();
            VerifySingleton<SqlCeSqlGenerator>();
            VerifySingleton<ISqlStatementExecutor>();
            VerifySingleton<SqlCeTypeMapper>();
            
            VerifySingleton<SqlCeModelSource>();

            // SQL Server Ce scoped
            VerifyScoped<SqlCeModificationCommandBatchFactory>();
            VerifyScoped<SqlCeDataStoreServices>();
            VerifyScoped<SqlCeDataStore>();
            VerifyScoped<SqlCeDataStoreConnection>();
            VerifyScoped<SqlCeModelDiffer>();
            VerifyScoped<SqlCeMigrationSqlGenerator>();
            VerifyScoped<SqlCeDataStoreCreator>();
            VerifyScoped<SqlCeHistoryRepository>();

            VerifyCommonDataStoreServices();

            // Migrations
            VerifyScoped<IMigrationAssembly>();
            VerifyScoped<IHistoryRepository>();
            VerifyScoped<IMigrator>();
            VerifySingleton<IMigrationIdGenerator>();
            VerifyScoped<IModelDiffer>();
            VerifyScoped<IMigrationSqlGenerator>();
        }

        protected override IServiceCollection GetServices(IServiceCollection services = null)
        {
            return (services ?? new ServiceCollection())
                .AddEntityFramework()
                .AddSqlCe()
                .ServiceCollection();
        }

        protected override EntityOptions GetOptions()
        {
            return SqlCeTestHelpers.Instance.CreateOptions();
        }

        protected override DbContext CreateContext(IServiceProvider serviceProvider)
        {
            return SqlCeTestHelpers.Instance.CreateContext(serviceProvider);
        }
    }
}