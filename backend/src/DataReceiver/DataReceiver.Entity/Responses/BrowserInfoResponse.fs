namespace DataReceiver.Entity.Responses

/// Browser info response model
type BrowserInfoResponse =
    { UserAgent: string option
      Platform: string option
      Language: string option }
