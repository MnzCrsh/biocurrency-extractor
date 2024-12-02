namespace DataReceiver.Entity

/// Request item with device information
type CreateDeviceItem =
    { Kind: string option
      Label: string option
      DeviceId: string option }
