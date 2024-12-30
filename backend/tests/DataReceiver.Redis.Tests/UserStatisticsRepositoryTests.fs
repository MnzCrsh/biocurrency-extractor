module DataReceiver.Redis.Tests.UserStatisticsRepositoryTests

open AutoFixture.Xunit2
open DataReceiver.Entity
open DataReceiver.Redis.Abstraction
open DataReceiver.TestContainers
open DataReceiver.Fixture
open Microsoft.Extensions.DependencyInjection
open Xunit

/// UserStatisticsRepository tests
type UserStatisticsRepositoryTests(fixture: DataReceiverTestFactory, _containers: DataReceiverTestContainers) =
    [<Theory; AutoData>]
    let ``SaveAsync should add statistics to DB`` (request: CreateUserStatisticRequest) =
        async {
            // Arrange
            use scope = fixture.CreateScope()

            let repository =
                scope.ServiceProvider.GetRequiredService<IUserStatisticsRepository>()

            // Act
            let! createRes = repository.SaveAsync request

            // Assert
            match createRes with
            | Ok response ->
                let! getByIdRes = repository.GetByIdAsync response.Id
                Assert.NotNull getByIdRes
            | Error errorValue -> Assert.Fail($"Expected Ok but got Error: {errorValue}")
        }

    [<Theory; AutoData>]
    let ``GetByIdAsync should return correct statistics by ID`` (request: CreateUserStatisticRequest) =
        async {
            // Arrange
            use scope = fixture.CreateScope()

            let repository =
                scope.ServiceProvider.GetRequiredService<IUserStatisticsRepository>()

            // Act
            let! createRes = repository.SaveAsync request

            match createRes with
            | Ok createResponse ->
                let! getByIdRes = repository.GetByIdAsync createResponse.Id

                // Assert
                Assert.NotNull getByIdRes

                match getByIdRes with
                | Ok getByIdResponse -> Assert.Equal(createResponse.Id, getByIdResponse.Id)

                | Error errorValue -> Assert.Fail $"Expected Ok but got Error: {errorValue}"
            | Error errorValue -> Assert.Fail $"Expected Ok but got Error: {errorValue}"
        }

    //TODO: Think about merging into one fixture class via composition
    interface IClassFixture<DataReceiverTestFactory>
    interface IClassFixture<DataReceiverTestContainers>
