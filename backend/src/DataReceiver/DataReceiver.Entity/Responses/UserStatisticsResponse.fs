namespace DataReceiver.Entity

open System
open DataReceiver.Entity.Responses

/// Response model that contains user-related statistics
type UserStatisticsResponse =
    { Id: Guid
      BrowserInfo: BrowserInfoResponse option
      ScreenInfo: ScreenInfoResponse option
      Geolocation: GeolocationResponse option
      MediaDevices: MediaDevicesResponse option
      NetworkInfo: NetworkInfoResponse option }
