/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BarrierLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
*/

using System;
using System.IO;
using BarrierLayer.Barriers;
using BarrierLayer.Db;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
        opt.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
    var dir = AppContext.BaseDirectory;
    var xmlPath = Path.Combine(dir, "doc.xml");
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddScoped<BarrierFacadeFactory>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<BarrierService>();
builder.Services.AddScoped<GuestBarrierService>();

var app = builder.Build();
app.UsePathBase("/barrier");
//app.UseHttpLogging();
var extensionProvider = new FileExtensionContentTypeProvider();
extensionProvider.Mappings.Add(".vue", "text/html");
app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = extensionProvider
});
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapControllerRoute("", "~/", new {controller = "home", action = "index"});
app.MapControllerRoute("guest", "~/ui/guest/{id:guid}", new {controller = "home", action = "index"});
app.MapControllerRoute("guestAdmin", "~/ui/admin/guest", new {controller = "home", action = "index"});

await using (var scope = app.Services.CreateAsyncScope())
{
    Console.WriteLine("Check Db migrations");
    var dbUser = scope.ServiceProvider.GetRequiredService<DomainContext>();
    await dbUser.Database.MigrateAsync();
}

app.Run();