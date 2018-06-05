using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Front.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        [Route("users/login")]
        [Route("users/register")]
        [Route("~/")]
        public ActionResult Index()
        {
            return View();
        }
    }
}