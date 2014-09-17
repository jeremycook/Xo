using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Xo.Startup))]
namespace Xo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
