using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(MonitoringSitesService.Startup1))]

namespace MonitoringSitesService
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // конфигурируем WebApi

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: null,
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });
            app.UseWebApi(config);
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
