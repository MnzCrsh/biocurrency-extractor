namespace DataReceiver.Entity.Responses

/// Generic wrapper with data for api response
type ResponseWrapper<'T> =
    { Data: 'T option
      Status: string option
      ErrorMessage: string option }
