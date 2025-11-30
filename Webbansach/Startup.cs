using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Webbansach.Startup))]
namespace Webbansach
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
