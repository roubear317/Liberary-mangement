using libraryManagment.api.Extention_Services;
using libraryManagment.api.Seed;
using libraryManagment.Core.Model;
using libraryManagment.Core.Services;
using libraryManagment.EF.Context;
using libraryManagment.EF.Repos;
using LibraryManagment.EF.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbcontext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBookRepository,BookRepository>();
builder.Services.AddScoped<IBookService,BookService>();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbcontext>().AddDefaultTokenProviders();

builder.Services.AddIdentityService(builder.Configuration);




var app = builder.Build();

//using(var scope = app.Services.CreateScope())
//{
//    var scopeServices = scope.ServiceProvider;

//    var context = scopeServices.GetRequiredService<AppDbcontext>();

//    var logger = scopeServices.GetRequiredService<ILoggerFactory>();

//    try
//    {



//        context.Database.MigrateAsync();



//    }




//}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
