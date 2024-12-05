namespace DataReceiver.Redis

open Microsoft.Extensions.DependencyInjection

/// Extension method for DataReceiver.Redis module
module RedisExtensions =

    type IServiceCollection with
        /// Adds Redis module to DI container. This method will add repositories and redis related services
        member services.AddRedisModule(connectionString: string) =
            RedisServices.RegisterServices services connectionString

            services
