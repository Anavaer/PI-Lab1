using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Lab.Startup))]
namespace MVC_Lab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
