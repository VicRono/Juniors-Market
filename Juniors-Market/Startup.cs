using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Juniors_Market.Startup))]
namespace Juniors_Market
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
