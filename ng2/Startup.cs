using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ng2
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add CORS to accept calls from standard Angular CLI running 'ng start' on http://localhost:4200
      services.AddCors();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

      // In production, the Angular files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/dist";
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      // Add CORS to accept calls from standard Angular CLI running 'ng start' on http://localhost:4200
      app.UseCors(builder => builder.WithOrigins("http://localhost:4200"));

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
          name: "default",
          template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "ClientApp";

        // Read: Drawbacks of SSR (Server-side rendering) - https://docs.microsoft.com/en-us/aspnet/core/spa/angular?view=aspnetcore-2.1&tabs=visual-studio#drawbacks-of-ssr
        //spa.UseSpaPrerendering(options =>
        //{
        //  options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.bundle.js";
        //  options.BootModuleBuilder = env.IsDevelopment()
        //      ? new AngularCliBuilder(npmScript: "build:ssr")
        //      : null;
        //  options.ExcludeUrls = new[] { "/sockjs-node" };
        //});

        if (env.IsDevelopment())
        {
          //spa.UseAngularCliServer(npmScript: "start");

          // To use the external Angular CLI instance instead of launching one of its own, replace the spa.UseAngularCliServer invocation with the following:
          spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        }
      });
    }
  }
}
