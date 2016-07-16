using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuotesPage_Site.Startup))]
namespace QuotesPage_Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
