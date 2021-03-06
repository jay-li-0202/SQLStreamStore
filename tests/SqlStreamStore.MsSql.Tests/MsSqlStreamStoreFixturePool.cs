﻿namespace SqlStreamStore
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Abstractions;

    public class MsSqlStreamStoreFixturePool : IAsyncLifetime
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<MsSqlStreamStoreFixture>> _fixturePoolBySchema
            = new ConcurrentDictionary<string, ConcurrentQueue<MsSqlStreamStoreFixture>>();

        public async Task<MsSqlStreamStoreFixture> Get(
            ITestOutputHelper outputHelper,
            string schema = "dbo")
        {
            var fixturePool = _fixturePoolBySchema.GetOrAdd(
                schema,
                _ => new ConcurrentQueue<MsSqlStreamStoreFixture>());

            if (!fixturePool.TryDequeue(out var fixture))
            {
                var dbUniqueName = (DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds;
                var databaseName = $"sss-v2-{dbUniqueName}";
                var dockerInstance = new DockerMsSqlServerDatabase(databaseName);
                await dockerInstance.CreateDatabase();

                fixture = new MsSqlStreamStoreFixture(
                    schema,
                    dockerInstance,
                    databaseName,
                    onDispose:() => fixturePool.Enqueue(fixture));

                outputHelper.WriteLine($"Using new fixture with db {databaseName}");
            }
            else
            {
                outputHelper.WriteLine($"Using pooled fixture with db {fixture.DatabaseName}");
            }

            await fixture.Prepare();

            return fixture;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}