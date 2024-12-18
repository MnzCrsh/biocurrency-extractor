module DataReceiver.ContainerTestBase

open System.Threading.Tasks
open DotNet.Testcontainers.Builders
open DotNet.Testcontainers.Containers
open Xunit

/// Test containers orchestrator
type DataReceiverTestContainers() =

    /// Redis image name
    let redisImage = "redis:latest"

    let network = NetworkBuilder().Build()

    /// Redis test container
    let redisContainer =
        ContainerBuilder()
            .WithImage(redisImage)
            .WithPortBinding(6379, 6379) // Map port 6379:6379
            .WithNetwork(network)
            .WithCleanUp(true)
            .Build()

    /// Starts containers in separate threads
    let startTestContainers () =
        let startContainerAsync (container: IContainer) =
            async { do! container.StartAsync() |> Async.AwaitTask }

        let containers = [ redisContainer ]

        containers
        |> List.map startContainerAsync
        |> List.map Async.StartAsTask
        |> Task.WhenAll
        |> Async.AwaitTask

    // Used to provide asynchronous lifetime functionality
    interface IAsyncLifetime with
        member _.InitializeAsync() = task { do! startTestContainers }

        member _.DisposeAsync() =
            task { do! redisContainer.DisposeAsync() }
