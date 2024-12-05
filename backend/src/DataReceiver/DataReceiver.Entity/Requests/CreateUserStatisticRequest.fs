namespace DataReceiver.Entity

open System

/// Request to create record with acquired user statistics scraped via frontend client
type CreateUserStatisticRequest =
    { UserId: Guid
      BrowserInfo: CreateBrowserInfoItem option
      ScreenInfo: CreateScreenInfoItem option
      Geolocation: CreateGeolocationItem option
      MediaDevices: CreateMediaDevicesItem option
      NetworkInfo: CreateNetworkInfoItem option }
