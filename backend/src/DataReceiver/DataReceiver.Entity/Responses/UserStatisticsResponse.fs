namespace DataReceiver.Entity

open DataReceiver.Entity.Responses

/// Response model that contains user-related statistics
type UserStatisticsResponse =
    { BrowserInfo: BrowserInfoResponse option
      ScreenInfo: ScreenInfoResponse option
      Geolocation: GeolocationResponse option
      MediaDevices: MediaDevicesResponse option
      NetworkInfo: NetworkInfoResponse option }
