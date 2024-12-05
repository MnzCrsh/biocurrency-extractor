module DataReceiver.Redis.Repository

open System.Text.Json
open DataReceiver.Entity
open DataReceiver.Redis.Abstraction
open StackExchange.Redis

/// Maps create request to response model with additional information
let private MapToResponse (request: CreateUserStatisticRequest) : UserStatisticsResponse =
    { DeviceInfo = failwith "todo"
      BrowserInfo = failwith "todo"
      ScreenInfo = failwith "todo"
      Geolocation = failwith "todo"
      MediaDevices = failwith "todo"
      NetworkInfo = failwith "todo" }

type UserStatisticsRepository(redis: IConnectionMultiplexer) =
    interface IUserStatisticsRepository with
        member this.SaveAsync request =
            async {
                let db = redis.GetDatabase()
                let redisKey = $"user:statistic:{request.UserId}"
                let serializedRequest = JsonSerializer.Serialize(request)

                try
                    let! result =
                        db.StringSetAsync(redisKey, serializedRequest)
                        |> Async.AwaitTask

                    if result then
                        return MapToResponse request |> Ok
                    else
                        return "Failed to save user statistics to Redis" |> Error
                with
                | ex -> return Error($"Error serializing or saving data: {ex.Message}")
            }
