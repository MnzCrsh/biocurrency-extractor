module DataReceiver.ContainerTestBase

open System.Threading.Tasks
open DotNet.Testcontainers.Builders
open DotNet.Testcontainers.Containers
open Xunit
open Xunit.Abstractions

type DataReceiverTestContainers(output: ITestOutputHelper) =

    /// Private client network
    let network = NetworkBuilder().Build()

    let redisContainer =
        let redisImage = "redis:latest"
        let redisPort = 6379

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

    let logContainerState (container: IContainer) =
        output.WriteLine($"Container {container.Name} logs:\n{container.GetLogsAsync() |> Async.AwaitTask}")
        output.WriteLine($"Container {container.Name} state: {container.State}")

    /// Starts containers in separate threads.
    let startTestContainers () : Task<unit array> =
        let startContainerAsync (container: IContainer) =
            async {
                output.WriteLine $"Starting container {container.Name}"
                do! container.StartAsync() |> Async.AwaitTask
                logContainerState container
            }

        let containers = [ redisContainer ]

        containers
        |> List.map startContainerAsync
        |> List.map Async.StartAsTask
        |> Task.WhenAll

    interface IAsyncLifetime with
        member _.InitializeAsync() = startTestContainers ()

        member _.DisposeAsync() =
            task {
                do! redisContainer.DisposeAsync()
                output.WriteLine "All Test containers has been disposed"
            }
