using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Definition for cookie
builder.Services.AddAuthentication("default")
    .AddCookie("default", o =>
    {
        o.Cookie.Name = "MyCookie";
        //o.Cookie.Domain = ""; // request the cookie in specific domaim
        //o.Cookie.Path = "/test"; // request the cookie in specific path
        o.Cookie.HttpOnly = true; // If httpOnly is false it means that user can get access to it by java script. but if it is true they can not get access to it (document.cookie)
        //o.Cookie.SameSite = SameSiteMode.Lax // This is Related to using Cookies in other website or something like that
        o.ExpireTimeSpan = TimeSpan.FromHours(1);// showing the expier time
        //o.ExpireTimeSpan = TimeSpan.FromSeconds(5);
        o.SlidingExpiration = true; //if this is true, when ewer a rewuest send,it refresh the exoier time and start it again. but if the cooki expiered, the request will fail
    });


// Create a policy here, I fon't know how to use it now exactly
builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("Mypolicy", pb => pb
    .RequireAuthenticatedUser()
    .RequireClaim("doesntExist", "nonse")
    );
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// This tow are requiered 
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/test", () => "Hello World").RequireAuthorization();

// You can call a chalange here for just an spesific APIs/ I dont lnow how to do that for all ather APIs/ (this challenge call in any situation i think)
app.MapGet("/test22", async (HttpContext ctx) =>
{
    await ctx.ChallengeAsync("default",
    new AuthenticationProperties()
    {
        RedirectUri = "/my-own-URL",// 
    });
    return "ok";
});


// You can impliment your login APIs here or in the controller.
app.MapPost("/login", async (HttpContext ctx) =>
{
    await ctx.SignInAsync("default", new ClaimsPrincipal(
    new ClaimsIdentity(
        new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier , Guid.NewGuid().ToString())
        },
        "default")
    ),
    
    new AuthenticationProperties()
    {
        IsPersistent = true,// showing that this cookie will expier
    });
    return "ok";
});


// You can impliment your logout APIs here or in the controller.
app.MapGet("/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync("default", 
    new AuthenticationProperties()
    {
        IsPersistent = true,// showing that this cookie will expier
    });
    return "ok";
});



app.MapDefaultControllerRoute();

app.MapControllers();

app.Run();
