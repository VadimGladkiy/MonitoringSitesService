using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin;
using System.Web.Http;

namespace MonitoringSitesService
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // конфигурируем WebApi
           
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });
            app.UseWebApi(config);
        }
    }
}
