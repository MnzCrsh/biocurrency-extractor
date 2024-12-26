module DataReceiver.Redis.Tests.UserStatisticsRepositoryTests

open DataReceiver.ContainerTestBase
open DataReceiver.Fixture
open FsUnit.Xunit
open Microsoft.Extensions.DependencyInjection
open StackExchange.Redis
open Xunit

/// UserStatisticsRepository tests
type UserStatisticsRepositoryTests(fixture: DataReceiverTestFactory, containers: DataReceiverTestContainers) =
    [<Fact(DisplayName = "Redis connection is established successfully")>]
    let ``Redis is reachable`` () =
        use scope = fixture.CreateScope()
        use multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>()
        multiplexer.IsConnected |> should be True

    //TODO: Think about merging into one fixture class via composition
    interface IClassFixture<DataReceiverTestFactory>
    interface IClassFixture<DataReceiverTestContainers>
