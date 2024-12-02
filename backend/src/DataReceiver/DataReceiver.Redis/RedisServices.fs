namespace DataReceiver.Redis

open Microsoft.Extensions.DependencyInjection
open StackExchange.Redis

/// DataReceiver.Redis dependency injection configuration
module RedisServices =

    /// Register redis related services
    let private AddRedis (services: IServiceCollection) (connString: string) =
        services.AddSingleton<IConnectionMultiplexer> (fun _ ->
            ConnectionMultiplexer.Connect(connString) :> IConnectionMultiplexer)
        |> ignore

    /// Register module repositories
    let private AddRepositories (services: IServiceCollection) =
        services.AddScoped<IUserStatisticsRepository, UserStatisticsRepository>()
        |> ignore

    /// Register DataReceiver.Redis module services
    let internal RegisterServices (services: IServiceCollection) (connString: string) =
        AddRedis services connString
        AddRepositories services
