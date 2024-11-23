namespace DataReceiver.Api.Requests

/// Request item with device information
type CreateDeviceItem =
    { Kind: string
      Label: string
      DeviceId: string }
