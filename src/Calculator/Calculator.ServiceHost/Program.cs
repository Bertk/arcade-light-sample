using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Calculator.ServiceHost
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

#pragma warning disable S125 // Sections of code should not be commented out
                    //ConfigureInsecureHttp2Listener(webBuilder);

                    webBuilder.UseStartup<Startup>();
#pragma warning restore S125 // Sections of code should not be commented out
                });

#pragma warning disable S1144 // Unused private types or members should be removed
        private static void ConfigureInsecureHttp2Listener(IWebHostBuilder webBuilder)
#pragma warning restore S1144 // Unused private types or members should be removed
        {
            // Makes Grpc.Core client with ChannelCredentials.Insecure work 
            webBuilder.ConfigureKestrel(options =>
            {
                // This endpoint will use HTTP/2 and HTTPS on port 5001.
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });
        }
    }
}
