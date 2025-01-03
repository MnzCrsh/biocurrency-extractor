﻿namespace DataReceiver.Entity

/// <summary>
/// Request item with browser provided meta information
/// </summary>
type CreateBrowserInfoItem =
    { UserAgent: string option
      Platform: string option
      Language: string option }
