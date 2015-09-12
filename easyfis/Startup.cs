using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(easyfis.Startup))]
namespace easyfis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
