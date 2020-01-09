using Microsoft.AspNetCore.Mvc;

namespace Front.React.Controllers
{
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0)]
    public class ControllerBase : Controller
    {
    }
}
