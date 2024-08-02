using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Managers
{
    internal class RedisManager
    {
        private static string endPoint = "redis-14389.c323.us-east-1-2.ec2.cloud.redislabs.com";
        private static string password = "rYuWZAWdISulGp9I2AbVho0wuzf8j155";
        private static int port = 14389;

        private static Lazy<ConnectionMultiplexer> lazyConnectionMultiplexer;
        static RedisManager()
        {
            RedisManager.lazyConnectionMultiplexer = new Lazy<ConnectionMultiplexer>(() =>
            {
                var configurationOptions = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { { endPoint, port } }
                    ,
                    Password = password
                    ,
                    ConnectTimeout = 10000
                };
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
        }

        public static ConnectionMultiplexer connection
        {
            get
            {
                return lazyConnectionMultiplexer.Value;
            }
        }
    }
}
