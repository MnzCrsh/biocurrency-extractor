module DataReceiver.ContainerTestBase

open System.Threading.Tasks
open DotNet.Testcontainers.Builders
open DotNet.Testcontainers.Containers


type DataReceiverTestContainers() =

    /// Redis image name
    let redisImage = "redis:latest"

    /// Private network
    let network = NetworkBuilder().Build()

    /// Redis test container
    let redisContainer =
        ContainerBuilder()
            .WithImage(redisImage)
            .WithPortBinding("6379")
            .WithNetwork(network)
            .WithCleanUp(true)
            .Build()

    /// Starts containers in separate threads from thread pool
    let startTestContainers () =
        let startContainerAsync (container: IContainer) =
            async { do! container.StartAsync() |> Async.AwaitTask }

        let containers = [ redisContainer ]

        containers
        |> List.map startContainerAsync
        |> List.map Async.StartAsTask
        |> Task.WhenAll
        |> Async.AwaitTask
