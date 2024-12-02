namespace DataReceiver.Redis

open System.Threading.Tasks
open DataReceiver.Entity
open StackExchange.Redis

type UserStatisticsRepository =
    interface IUserStatisticsRepository with
        member this.SaveAsync (request:CreateUserStatisticRequest) =
            new Task<UserStatisticsResponse>
