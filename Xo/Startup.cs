using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Xo.Startup))]
namespace Xo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // TODO: May want to wire IOC in here instead of in MvcApplication.Application_Start.
            // Note that this runs before MvcApplication.Application_Start.

            ConfigureIdentity(app);
        }
    }
}
