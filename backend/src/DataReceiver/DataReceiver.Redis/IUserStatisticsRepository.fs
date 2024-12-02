namespace DataReceiver.Redis

open System.Threading.Tasks
open DataReceiver.Entity


/// Repository for interaction with user statistics
type IUserStatisticsRepository =
    /// Saves user statistics to database asynchronously
    abstract member SaveAsync: CreateUserStatisticRequest -> Task<UserStatisticsResponse>
