﻿// <copyright company="PetaPoco - CollaboratingPlatypus">
//      Apache License, Version 2.0 https://github.com/CollaboratingPlatypus/PetaPoco/blob/master/LICENSE.txt
// </copyright>
// <author>PetaPoco - CollaboratingPlatypus</author>
// <date>2015/12/28</date>

using System;
using System.Data;
using PetaPoco.Core;
using PetaPoco.Providers;
using Shouldly;
using Xunit;

namespace PetaPoco.Tests.Unit
{
    public class DatabaseConfigurationTests
    {
        private readonly IDatabaseBuildConfiguration config;
        private const string ConnectionString = "ConnectionString";

        public DatabaseConfigurationTests()
        {
            config = DatabaseConfiguration.Build();
        }

        [Fact]
        public void SetSetting_GivenKeyAndValue_ShouldSetSetting()
        {
            ((IBuildConfigurationSettings)config).SetSetting(key: "key", value: "value");

            string value = null;

            ((IBuildConfigurationSettings) config).TryGetSetting<string>(key: "key", setSetting: v => value = v);

            value.ShouldNotBeNull();
            value.ShouldBe("value");
        }

        [Fact]
        public void SetSetting_GivenKeyAndNullValue_ShouldRemoveSetting()
        {
            ((IBuildConfigurationSettings) config).SetSetting(key: "key", value: "value");
            ((IBuildConfigurationSettings) config).SetSetting(key: "key", value: null);
            var getCalled = false;

            ((IBuildConfigurationSettings)config).TryGetSetting<string>(
                key: "key",
                setSetting: v => { getCalled = true; });

            getCalled.ShouldBeFalse();
        }

        [Fact]
        public void TryGetSetting_GivenKeyAndValue_ShouldGetSetting()
        {
            string value = "value";
            ((IBuildConfigurationSettings)config).SetSetting(key: "key", value: value);

            ((IBuildConfigurationSettings)config).TryGetSetting<string>(
                key: "key",
                setSetting: v => value = v);

            value.ShouldNotBeNull();
            value.ShouldBe(value);
        }

        [Fact]
        public void TryGetSetting_GivenKeyThatDoesNotMatchValue_ShouldNotCallback()
        {
            var getCalled = false;

            ((IBuildConfigurationSettings)config).TryGetSetting<string>(
                key: "key",
                setSetting: v => { getCalled = true; });

            getCalled.ShouldBeFalse();
        }

        [Fact]
        public void TrySetSetting_GivenNullKey_Throws()
        {
            Should.Throw<ArgumentNullException>(
                () => ((IBuildConfigurationSettings) config).SetSetting(
                    key: null,
                    value: "value"));
        }

        [Fact]
        public void TryGetSetting_GivenNullKey_Throws()
        {
            Should.Throw<ArgumentNullException>(
                () => ((IBuildConfigurationSettings) config).TryGetSetting<string>(
                    key: null,
                    setSetting: v => { }));
        }

        [Fact]
        public void TryGetSetting_GivenNullCallback_Throws()
        {
            ((IBuildConfigurationSettings) config).SetSetting(key: "key", value: "value");

            Should.Throw<NullReferenceException>(
                () => ((IBuildConfigurationSettings) config).TryGetSetting<string>(
                    key: "key",
                    setSetting: null));
        }

        [Fact]
        public void UsingCreate_GivenMinimalConfiguration_ShouldNotAffectPetaPoocDefaults()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .Create();

            db.CommandTimeout.ShouldBe(0);
            db.Provider.ShouldBeOfType<SqlServerDatabaseProvider>();
            db.ConnectionString.ShouldBe(ConnectionString);
            db.DefaultMapper.ShouldBeOfType<ConventionMapper>();
            db.EnableAutoSelect.ShouldBeTrue();
            db.EnableNamedParams.ShouldBeTrue();
        }

        [Fact]
        public void UsingCommandTimeout_GivenInvalidArguments_Throws()
        {
            Should.Throw<ArgumentException>(() => config.UsingCommandTimeout(0));
            Should.Throw<ArgumentException>(() => config.UsingCommandTimeout(-1));
        }

        [Fact]
        public void UsingCommandTimeout_GivenTimeoutAndAfterCreate_ShouldBeSameAsPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingCommandTimeout(50)
                .Create();

            db.CommandTimeout.ShouldBe(50);
        }

        [Fact]
        public void WithNamedParams_AfterCreate_ShouldBeSetOnPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .WithNamedParams()
                .Create();

            db.EnableNamedParams.ShouldBeTrue();
        }

        [Fact]
        public void WithoutNamedParams_AfterCreate_ShouldNotBeSetOnPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .WithoutNamedParams()
                .Create();

            db.EnableNamedParams.ShouldBeFalse();
        }

        [Fact]
        public void WithAutoSelect_AfterCreate_ShouldBeSetOnPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .WithAutoSelect()
                .Create();

            db.EnableNamedParams.ShouldBeTrue();
        }

        [Fact]
        public void WithoutAutoSelect_AfterCreate_ShouldNotBeSetOnPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .WithoutAutoSelect()
                .Create();

            db.EnableAutoSelect.ShouldBeFalse();
        }

        [Fact]
        public void UsingConnectionString_GivenInvalidArguments_Throws()
        {
            Should.Throw<ArgumentException>(() => config.UsingConnectionString(null));
            Should.Throw<ArgumentException>(() => config.UsingConnectionString(String.Empty));
        }

        [Fact]
        public void UsingConnectionString_GivenTimeoutAndAfterCreate_ShouldBeSameAsPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .Create();

            db.ConnectionString.ShouldBe(ConnectionString);
        }

        [Fact]
        public void UsingConnectionStringName_GivenInvalidArguments_Throws()
        {
            Should.Throw<ArgumentException>(() => config.UsingConnectionStringName(null));
            Should.Throw<ArgumentException>(() => config.UsingConnectionStringName(String.Empty));
        }

        [Fact(Skip = "Can't be tested as testing would require connection strings in the app/web config.")]
        public void UsingConnectionStringName_GivenTimeoutAndAfterCreate_ShouldBeSameAsPetaPocoInstance()
        {
        }

        [Fact]
        public void UsingDefaultMapper_GivenInvalidArguments_Throws()
        {
            Should.Throw<ArgumentNullException>(() => config.UsingDefaultMapper((StandardMapper) null));
            Should.Throw<ArgumentNullException>(() => config.UsingDefaultMapper((StandardMapper) null, null));
            Should.Throw<ArgumentNullException>(() => config.UsingDefaultMapper(new ConventionMapper(), null));
            Should.Throw<ArgumentNullException>(() => config.UsingDefaultMapper((Action<StandardMapper>) null));
        }

        [Fact]
        public void UsingDefaultMapper_GivenMapperOrType_ShouldBeSameAsPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingDefaultMapper(new StandardMapper())
                .Create();

            var db1 = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingDefaultMapper<StandardMapper>()
                .Create();

            db.DefaultMapper.ShouldBeOfType<StandardMapper>();
            db1.DefaultMapper.ShouldBeOfType<StandardMapper>();
        }

        [Fact]
        public void UsingDefaultMapper_GivenMapperOrTypeAndConfigurationCallback_ShouldBeSameAsPetaPocoInstanceAndCallback()
        {
            var dbCalled = false;
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingDefaultMapper(new StandardMapper(), sm => dbCalled = true)
                .Create();

            var db1Called = false;
            var db1 = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingDefaultMapper<StandardMapper>(sm => db1Called = true)
                .Create();

            dbCalled.ShouldBeTrue();
            db.DefaultMapper.ShouldBeOfType<StandardMapper>();
            db1Called.ShouldBeTrue();
            db1.DefaultMapper.ShouldBeOfType<StandardMapper>();
        }

        [Fact]
        public void UsingIsolationLevel_GivenIsolationLevelAndAfterCreate_ShouldBeSameAsPetaPocoInstance()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .UsingIsolationLevel(IsolationLevel.Chaos)
                .Create();

            db.IsolationLevel.ShouldBe(IsolationLevel.Chaos);
        }

        [Fact]
        public void NotUsingIsolationLevel_AfterCreate_PetaPocoInstanceShouldBeNull()
        {
            var db = config
                .UsingConnectionString(ConnectionString)
                .UsingProvider<SqlServerDatabaseProvider>()
                .Create();

            db.IsolationLevel.ShouldBeNull();
        }
    }
}