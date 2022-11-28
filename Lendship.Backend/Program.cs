using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Lendship.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseUrls("http://0.0.0.0:5000");
                    webBuilder.UseStartup<Startup>();
                    /*webBuilder.UseKestrel((hostingContext, options) =>
                    {
                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            options.Listen(IPAddress.Loopback, 9001);
                            options.Listen(IPAddress.Loopback, 9002, listenOptions =>
                            {
                                listenOptions.UseHttps("certificate.pfx", "LendshipCertPassword");
                            });
                        }

                    });*/
                });
        }
    }
}
