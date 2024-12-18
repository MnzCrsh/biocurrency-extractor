module DataReceiver.Redis.Tests.UserStatisticsRepositoryTests

open DataReceiver.Fixture
open Xunit

/// UserStatisticsRepository tests
type UserStatisticsRepositoryTests() =
    inherit DataReceiverFixtureFactory<Program.Startup>()

    [<Fact(DisplayName = "Redis connection is established successfully")>]
    let ``Redis connection is established`` () = ""
