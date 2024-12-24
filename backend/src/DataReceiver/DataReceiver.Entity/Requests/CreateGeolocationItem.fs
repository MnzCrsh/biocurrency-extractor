namespace DataReceiver.Entity

/// Request item with coordinates provided by browser
type CreateGeolocationItem =
    { Longitude: decimal option
      Latitude: decimal option }
