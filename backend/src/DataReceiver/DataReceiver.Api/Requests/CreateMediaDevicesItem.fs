namespace DataReceiver.Api.Requests

open System.Collections.Generic

/// Request item with information about user physical devices
type CreateMediaDevicesItem =
    { MemoryGb: int option
      ConnectedDevices: IEnumerable<CreateDeviceItem> option }
