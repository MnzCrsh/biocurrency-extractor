module DataReceiver.Fixture

open DataReceiver.Redis.RedisExtensions
open Microsoft.AspNetCore.Mvc.Testing
open Microsoft.Extensions.DependencyInjection

/// Private integration testing factory, the place where things
/// such as Services Collection, API settings and DB connections are configured
type private DataReceiverTestFactoryPrivate() =
    inherit WebApplicationFactory<Program.CoreEntry>()

    let redisConnectionString = "localhost:6379,abortConnect=false"

    member _.CreateScopeInternal() =
        ``base``
            .Services
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope()

    override _.ConfigureWebHost(builder) =
        ``base``.ConfigureWebHost(builder)

        builder.ConfigureServices (fun services ->
            redisConnectionString
            |> services.AddRedisModule
            |> ignore)
        |> ignore

    override this.CreateHost(builder) =
        let host = ``base``.CreateHost(builder)
        host

/// Integration testing factory that provides simpler access to DI services and
/// implicitly manages things such as Services Collection, API settings and DB connections
type DataReceiverTestFactory() =
    let privateFactory = new DataReceiverTestFactoryPrivate()

    /// Create a IServiceScope that contains a System.
    /// IServiceProvider used to resolve dependencies from a newly created scope.
    /// Should be disposed.
    member _.CreateScope() = privateFactory.CreateScopeInternal()
