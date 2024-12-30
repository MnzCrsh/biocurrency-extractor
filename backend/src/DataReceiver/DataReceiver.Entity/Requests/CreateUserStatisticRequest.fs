namespace DataReceiver.Entity

// TODO: Must add JWT identification, because right now statistics not bound to user
/// Request to create record with acquired user statistics scraped via frontend client
type CreateUserStatisticRequest =
    { BrowserInfo: CreateBrowserInfoItem option
      ScreenInfo: CreateScreenInfoItem option
      Geolocation: CreateGeolocationItem option
      MediaDevices: CreateMediaDevicesItem option
      NetworkInfo: CreateNetworkInfoItem option }
