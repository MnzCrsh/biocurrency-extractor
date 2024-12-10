module DataReceiver.Redis.Repository

open System.Text.Json
open DataReceiver.Entity
open DataReceiver.Entity.Responses
open DataReceiver.Redis.Abstraction
open StackExchange.Redis


/// Maps device create request to response model
let private MapDeviceItemToResponse (request: CreateDeviceItem) : DeviceItemResponse =
    { Kind = request.Kind
      Label = request.Label
      DeviceId = request.DeviceId }


/// Maps optional values with a mapping function
let private OptionMapSeq mapper optionSeq =
    optionSeq |> Option.map (Seq.map mapper)

/// Maps create request to response model
let private MapUserStatisticsToResponse (request: CreateUserStatisticRequest) : UserStatisticsResponse =
    { BrowserInfo =
        request.BrowserInfo
        |> Option.map (fun info ->
            { Language = info.Language
              Platform = info.Platform
              UserAgent = info.UserAgent })

      ScreenInfo =
          request.ScreenInfo
          |> Option.map (fun info ->
              { Width = info.Width
                Length = info.Length })

      Geolocation =
          request.Geolocation
          |> Option.map (fun info ->
              { Latitude = info.Latitude
                Longitude = info.Longitude })

      MediaDevices =
          request.MediaDevices
          |> Option.map (fun info ->
              { ConnectedDevices = OptionMapSeq MapDeviceItemToResponse info.ConnectedDevices
                MemoryGb = info.MemoryGb })

      NetworkInfo =
          request.NetworkInfo
          |> Option.map (fun info -> { ConnectionType = info.ConnectionType }) }

/// Repository implementation that asynchronously works with redis database
type UserStatisticsRepository(redis: IConnectionMultiplexer) =
    interface IUserStatisticsRepository with
        member this.SaveAsync request =
            async {
                let db = redis.GetDatabase()

                try
                    let! result =
                        let redisKey = $"user:statistic:{request.UserId}"
                        let serializedRequest = JsonSerializer.Serialize(request)

                        db.StringSetAsync(redisKey, serializedRequest)
                        |> Async.AwaitTask

                    if result then
                        return Ok <| MapUserStatisticsToResponse request
                    else
                        return Error <| "Failed to save user statistics to Redis"

                with
                | ex -> return Error($"Error serializing or saving data: {ex.Message}")
            }
