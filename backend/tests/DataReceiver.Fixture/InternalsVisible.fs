open System.Runtime.CompilerServices

module InternalsVisible =
    [<assembly: InternalsVisibleTo("DataReceiver.Redis.Tests")>]
    do ()
