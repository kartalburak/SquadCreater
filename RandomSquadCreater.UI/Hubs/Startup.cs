using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(RandomSquadCreater.UI.Hubs.Startup))]

namespace RandomSquadCreater.UI.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder App)
        {
            //Long pooling ile eski versiyon tarayıcılara izin veriyoruz.
            //var config = new ConnectionConfiguration()
            //{
            //    EnableJSONP = true
            //};
            //var hubConfiguration = new HubConfiguration();
            //hubConfiguration.EnableDetailedErrors = true;

            //App.Map("/signalr", map =>
            //{
            //    map.UseCors(CorsOptions.AllowAll);
            //    map.RunSignalR<ChatConnection>("/echo",hubConfiguration);
            //});



            //App.MapSignalR<ChatConnection>("/echo",hubConfiguration);



            App.MapSignalR();
        }
    }
}
