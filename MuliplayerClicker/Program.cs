using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Orleans.Hosting;

namespace MuliplayerClicker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseOrleans(siloBuilder =>
                {
                    siloBuilder.UseLocalhostClustering();

                    siloBuilder.AddAzureBlobGrainStorageAsDefault(options =>
                    {
                        options.UseJson = true;
                        options.ConnectionString = "UseDevelopmentStorage=true";
                        options.ContainerName = "clicker";
                    });

                    siloBuilder.AddSimpleMessageStreamProvider("ClickerStream");
                    siloBuilder.AddMemoryGrainStorage("PubSubStore");
                });
    }
}
