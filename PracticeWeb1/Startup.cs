using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PracticeWeb1.Startup))]
namespace PracticeWeb1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
