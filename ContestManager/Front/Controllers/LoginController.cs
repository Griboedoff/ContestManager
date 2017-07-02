using System.Web.Mvc;

namespace Front.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
            => View();
    }
}