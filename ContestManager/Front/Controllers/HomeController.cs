using System.Web.Http.Cors;
using System.Web.Mvc;

namespace Front.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}