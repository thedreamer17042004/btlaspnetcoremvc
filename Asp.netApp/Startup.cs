using Asp.netApp.Areas.Admin.Configs;
using AutoMapper;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Register AutoMapper and add the mapping profile
        // Register AutoMapper and add the mapping profile manually
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
       // app.UseMiddleware<CustomExceptionMiddleware>();
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseRequestLocalization();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStatusCodePages();
       app.UseStatusCodePages(async context =>
         {
             if (context.HttpContext.Response.StatusCode == 404)
             {
                 context.HttpContext.Response.Redirect("/Admin/Error/NotFound");
             }
             
         });


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
