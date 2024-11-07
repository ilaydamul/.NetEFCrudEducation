using Microsoft.EntityFrameworkCore;
using WebApiIdentity.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppIdentityContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Conn"));
});

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail= true;
    opt.Password.RequireDigit= true;
    opt.Password.RequireLowercase= true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength= 8;
    opt.SignIn.RequireConfirmedEmail= false; //true olursa login olmak için register mail onaylamalýyýz.

}).AddEntityFrameworkStores<AppIdentityContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseRouting();
//app.UsePathBase("api");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
