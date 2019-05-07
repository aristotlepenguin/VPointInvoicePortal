using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(attemptonemillion.Startup))]
namespace attemptonemillion
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
