using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GymManagementSystem.Startup))]
namespace GymManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
