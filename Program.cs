using Microsoft.AspNetCore.Authentication.Cookies;
using UseAuthTest.BL;
using UseAuthTest.BL.Interfaces;
using UseAuthTest.DAL;
using UseAuthTest.DAL.Helpers;
using UseAuthTest.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.Cookie.HttpOnly = true;
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IUserDAL, UserDAL>();
builder.Services.AddSingleton<IUserAuth, UserAuth>();
builder.Services.AddSingleton<IEncrypt, Encrypt>();

builder.Services.AddSingleton<ICurrentUser, CurrentUser>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

HelperDb.ConnectionString = app.Configuration.GetConnectionString("Dev") ?? string.Empty;

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
