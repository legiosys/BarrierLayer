using BarrierLayer.Barriers;
using BarrierLayer.Db;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace BarrierLayer
{
    public class Startup(IConfiguration configuration)
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase(configuration);
            services.AddScoped<BarrierFacadeFactory>();
            services.AddScoped<UserService>();
            services.AddScoped<ConfigService>();
            services.AddScoped<BarrierService>();
            services.AddScoped<GuestBarrierService>();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter()));
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "BarrierLayer Api", Version = "v1"});
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var extensionProvider = new FileExtensionContentTypeProvider();
            extensionProvider.Mappings.Add(".vue", "text/html");
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = extensionProvider
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "BarrierLayer API V1"); });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("", "~/", new {controller = "home", action = "index"});
                endpoints.MapControllerRoute("guest", "~/ui/guest/{id:guid}",
                    new {controller = "home", action = "index"});
                endpoints.MapControllerRoute("guestAdmin", "~/ui/admin/guest",
                    new {controller = "home", action = "index"});
            });
        }
    }
}