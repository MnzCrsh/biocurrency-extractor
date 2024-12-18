module DataReceiver.Redis.Abstraction

open System
open DataReceiver.Entity


/// Repository for interaction with user statistics
type IUserStatisticsRepository =
    /// Saves user statistics to database asynchronously
    abstract member SaveAsync: CreateUserStatisticRequest -> Async<Result<UserStatisticsResponse, string>>

    /// Gets statistics by user id
    abstract member GetById: Guid -> Async<Result<UserStatisticsResponse, string>>
