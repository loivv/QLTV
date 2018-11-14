using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LB.Startup))]
namespace LB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
