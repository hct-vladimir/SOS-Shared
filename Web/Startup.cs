using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountingSos.Startup))]
namespace AccountingSos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
