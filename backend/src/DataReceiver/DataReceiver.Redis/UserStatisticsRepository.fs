﻿module DataReceiver.Redis.Repository

open System
open System.Text.Json
open DataReceiver.Entity
open DataReceiver.Entity.Responses
open DataReceiver.Redis.Abstraction
open StackExchange.Redis


/// Maps device create request to response model
let private MapDeviceItemToResponse (request: CreateDeviceItem) =
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

/// Gets redis key by appending Guid to string template
let private CombineRedisKey (id: Guid) = $"user:statistic:{id}"

/// Repository implementation that asynchronously works with redis database
type UserStatisticsRepository(redis: IConnectionMultiplexer) =
    interface IUserStatisticsRepository with
        member _.SaveAsync request =
            let saveToRedis (db: IDatabase) (key: string, value: string) =
                async {
                    let! result = db.StringSetAsync(key, value) |> Async.AwaitTask
                    return result
                }

            async {
                try
                    let db = redis.GetDatabase()
                    let redisKey = request.UserId |> CombineRedisKey
                    let serializedRequest = request |> JsonSerializer.Serialize

                    let! result = saveToRedis db (redisKey, serializedRequest)

                    if result then
                        return Ok <| MapUserStatisticsToResponse request
                    else
                        return Error "Failed to save user statistics to Redis"

                with
                | ex -> return Error $"Error serializing or saving data: {ex.Message}"
            }

        member _.GetById(id) =
            async {
                try
                    let db = redis.GetDatabase()

                    let! redisString =
                        CombineRedisKey id
                        |> db.StringGetAsync
                        |> Async.AwaitTask

                    let deserializedResult =
                        redisString
                        |> JsonSerializer.Deserialize<UserStatisticsResponse>

                    return Ok deserializedResult

                with
                | ex -> return Error $"Unable to get data for user with ID:{id} with error {ex}"
            }
