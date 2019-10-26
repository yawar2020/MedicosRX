using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicosRX.Startup))]
namespace MedicosRX
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
