using Azure.Storage.Blobs;
using Lab4.Data;
using Microsoft.EntityFrameworkCore;

namespace King0483Assignment2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MarketDbContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();

            var blobConnection = Configuration.GetConnectionString("AzureBlobStorage");
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobConnection);
            services.AddSingleton(blobServiceClient);
            services.AddRazorPages();
            services.AddSession();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}