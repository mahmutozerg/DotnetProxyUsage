using System.Net;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using WebScrapping.Core;
using WebScrapping.Core.Repositories;
using WebScrapping.Core.Services;
using WebScrapping.Core.UnitOfWorks;
using WebScrapping.Repository;
using WebScrapping.Repository.Repositories;
using WebScrapping.Repository.UnitOfWorks;
using WebScrapping.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"), options =>
    {
        options.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
        options.EnableRetryOnFailure();
        
    });
});


builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<ProxyService>();

builder.Services.AddHttpClient<RequestService>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    Proxy = new WebProxy($"http://{ProxyConfig.ProxyAddress}:{ProxyConfig.ProxyPort}"),
    UseProxy = true
});

var app = builder.Build();

var proxyService = app.Services.GetRequiredService<ProxyService>();
proxyService.Start();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();