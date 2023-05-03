using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,opt =>
{
    opt.Cookie.HttpOnly= false;//Secure. Sadece https kanal�ndan aktif olacak.
    opt.Cookie.SameSite = SameSiteMode.Strict;
    opt.ExpireTimeSpan= TimeSpan.FromDays(30);
    opt.LoginPath = "/login";
    opt.LogoutPath = "/logout";
    opt.AccessDeniedPath= "/unathorized";
    opt.SlidingExpiration = true;//20dkda bir oturumu artt�r.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();//Kimlik do�rulamas� middleware aktif hale getiririz.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
