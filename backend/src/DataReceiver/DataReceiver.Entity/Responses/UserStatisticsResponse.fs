namespace DataReceiver.Entity

open DataReceiver.Entity.Responses

/// Response model that contains user-related statistics
type UserStatisticsResponse =
    { DeviceInfo: ResponseWrapper<MediaDevicesResponse>
      BrowserInfo: ResponseWrapper<BrowserInfoResponse>
      ScreenInfo: ResponseWrapper<ScreenInfoResponse>
      Geolocation: ResponseWrapper<GeolocationResponse>
      MediaDevices: ResponseWrapper<MediaDevicesResponse>
      NetworkInfo: ResponseWrapper<NetworkInfoResponse> }
