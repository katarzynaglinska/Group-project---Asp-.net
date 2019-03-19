using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uploader.Startup))]
namespace Uploader
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
