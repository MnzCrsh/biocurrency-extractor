module DataReceiver.Fixture

open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.DependencyInjection
open StackExchange.Redis

/// Fixture for integration testing. Mocks all dependencies and handles test container connections
type DataReceiverFixtureFactory<'TProgram when 'TProgram: not struct>() =
    inherit WebApplicationFactory<'TProgram>()

    let RedisConnectionString = "localhost:6379"

    override _.ConfigureWebHost(builder) =

        builder.ConfigureServices (fun services ->
            services.AddSingleton<IConnectionMultiplexer>(
                RedisConnectionString
                |> ConnectionMultiplexer.Connect
            )
            |> ignore)
        |> ignore
