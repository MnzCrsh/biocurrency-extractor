namespace DataReceiver.Entity.Responses

open System.Collections.Generic

///
type MediaDevicesResponse =
    { MemoryGb: int option
      ConnectedDevices: IEnumerable<DeviceItemResponse> option }
