﻿using Microsoft.EntityFrameworkCore.Specification.Tests;
using Microsoft.EntityFrameworkCore.Specification.Tests.Utilities;
using Microsoft.EntityFrameworkCore.TestModels.ComplexNavigationsModel;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class ComplexNavigationsOwnedQuerySqlCeFixture
        : ComplexNavigationsOwnedQueryRelationalFixtureBase<SqlCeTestStore>
    {
        public static readonly string DatabaseName = "ComplexNavigationsOwned";

        private readonly DbContextOptions _options;
        private readonly string _connectionString = SqlCeTestStore.CreateConnectionString(DatabaseName);

        public TestSqlLoggerFactory TestSqlLoggerFactory { get; } = new TestSqlLoggerFactory();

        public ComplexNavigationsOwnedQuerySqlCeFixture()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlCe()
                .AddSingleton(TestModelSource.GetFactory(OnModelCreating))
                .AddSingleton<ILoggerFactory>(TestSqlLoggerFactory)
                .BuildServiceProvider(validateScopes: true);

            _options = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging()
                .UseSqlCe(_connectionString, b => b.ApplyConfiguration())
                .UseInternalServiceProvider(serviceProvider).Options;
        }

        public override SqlCeTestStore CreateTestStore()
        {
            return SqlCeTestStore.GetOrCreateShared(DatabaseName, () =>
            {
                using (var context = new ComplexNavigationsContext(_options))
                {
                    context.Database.EnsureCreated();
                    ComplexNavigationsModelInitializer.Seed(context, tableSplitting: true);
                }
            });
        }

        public override ComplexNavigationsContext CreateContext(SqlCeTestStore testStore)
        {
            var context = new ComplexNavigationsContext(_options);

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            context.Database.UseTransaction(testStore.Transaction);

            return context;
        }
    }
}