module DataReceiver.ContainerTestBase

open System.Threading.Tasks
open DotNet.Testcontainers.Builders
open DotNet.Testcontainers.Containers
open Xunit

type DataReceiverTestContainers() =

    /// Private client network
    let network = NetworkBuilder().Build()

    let redisPort = 6379

    let redisContainer =
        let redisImage = "redis:latest"

        ContainerBuilder()
            .WithImage(redisImage)
            .WithPortBinding(redisPort, redisPort)
            .WithNetwork(network)
            .WithNetworkAliases("redis")
            .WithWaitStrategy(
                Wait
                    .ForUnixContainer()
                    .UntilPortIsAvailable(redisPort)
            )
            .WithCleanUp(true)
            .Build()


    /// Starts containers in separate threads.
    let startTestContainers () : Task<unit array> =
        let startContainerAsync (container: IContainer) =
            async { do! container.StartAsync() |> Async.AwaitTask }

        let containers = [ redisContainer ]

        containers
        |> List.map startContainerAsync
        |> List.map Async.StartAsTask
        |> Task.WhenAll

    member _.RedisConnectionString =
        $"localhost:{redisContainer.GetMappedPublicPort(6379)},abortConnect=false"

    interface IAsyncLifetime with
        member _.InitializeAsync() = startTestContainers ()

        member _.DisposeAsync() =
            task { do! redisContainer.DisposeAsync() }
