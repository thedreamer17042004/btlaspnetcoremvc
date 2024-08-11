using Asp.netApp.Areas.Admin.Common;
using Asp.netApp.Areas.Admin.Configs;
using Asp.netApp.Areas.Admin.Data;
using Asp.netApp.Areas.Admin.Exceptions;
using Asp.netApp.Areas.Admin.Services.Auth;
using Asp.netApp.Areas.Admin.Services.Role;
using Asp.netApp.Areas.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization()
    ;

builder.Services.AddSingleton<LanguageService>();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";

});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{

    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("vi-VN")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "vi-VN", uiCulture: "vi-VN");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

});

//to DI ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppContext")));


//register roleService for dependency injection
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<FileUpload>();


builder.Services.AddHttpContextAccessor();
//config cookie for authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        //options.AccessDeniedPath = "/Admin/Error/Forbidden/Forbidden.cshtml";
        options.LoginPath = "/Admin/Auth/Login";
    });
//register a exception


var mapper = MapConfig.InitializeAutomapper();
builder.Services.AddSingleton(mapper);
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper1 = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper1);

// Add a reference to the Startup class
builder.Services.AddRazorPages().AddApplicationPart(typeof(Startup).Assembly);

var app = builder.Build();

app.UseMiddleware<CustomExceptionMiddleware>();

// Configure the HTTP request pipeline.
var startup = new Startup(builder.Configuration);
startup.Configure(app, builder.Environment);

app.Run();
