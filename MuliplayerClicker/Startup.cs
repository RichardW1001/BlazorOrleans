using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MuliplayerClicker.Data;
using MuliplayerClicker.Services;

namespace MuliplayerClicker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddSingleton<ClickerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }

    //public class ClusterService : IHostedService
    //{
    //    private readonly ILogger<ClusterService> logger;

    //    public ClusterService(ILogger<ClusterService> logger)
    //    {
    //        this.logger = logger;

    //        Client = new ClientBuilder()
    //            .ConfigureApplicationParts(manager => manager.AddApplicationPart(typeof(IClickerGrain).Assembly).WithReferences())
    //            .UseLocalhostClustering()
    //            .AddSimpleMessageStreamProvider("ClickerStream")
    //            .Build();
    //    }

    //    public async Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        await Client.Connect(async error =>
    //        {
    //            logger.LogError(error, error.Message);
    //            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
    //            return true;
    //        });
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken) => Client.Close();

    //    public IClusterClient Client { get; }
    //}

    //public static class ClusterServiceBuilderExtensions
    //{
    //    public static IServiceCollection AddClusterService(this IServiceCollection services)
    //    {
    //        services.AddSingleton<ClusterService>();
    //        services.AddSingleton<IHostedService>(_ => _.GetService<ClusterService>());
    //        services.AddTransient(_ => _.GetService<ClusterService>().Client);
    //        return services;
    //    }
    //}
}
