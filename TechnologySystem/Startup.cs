using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TechnologySystem.Startup))]
namespace TechnologySystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
