module Program

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open DataReceiver.Redis.RedisExtensions

/// Passable EntryPoint
type CoreEntry() =

    [<EntryPoint>]
    static let main args =

        let exitCode = 0
        let builder = WebApplication.CreateBuilder(args)

        builder
            .Services
            .AddRedisModule(builder.Configuration.GetConnectionString("Redis"))
            .AddControllers()

        let app = builder.Build()

        app.UseHttpsRedirection()

        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        exitCode
