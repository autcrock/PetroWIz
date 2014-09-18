using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASPLASBrowse.Startup))]
namespace ASPLASBrowse
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
