module DataReceiver.Redis.Tests.UserStatisticsRepositoryTests

open DataReceiver.Fixture
open FsUnit.Xunit
open Microsoft.Extensions.DependencyInjection
open StackExchange.Redis
open Xunit

/// UserStatisticsRepository tests
type UserStatisticsRepositoryTests(fixture: DataReceiverTestFactory) =
    [<Fact>]
    let ``Redis is reachable`` () =
        let connection = ConnectionMultiplexer.Connect("localhost:6379, abortConnect=false")
        connection.IsConnected |> should be True


    [<Fact(DisplayName = "Redis connection is established successfully")>]
    let ``Redis connection is established`` () =
        use scope = fixture.CreateScope()
        use multiplexer = scope.ServiceProvider.GetRequiredService<IConnectionMultiplexer>()
        multiplexer.IsConnected |> should be True

    interface IClassFixture<DataReceiverTestFactory>
