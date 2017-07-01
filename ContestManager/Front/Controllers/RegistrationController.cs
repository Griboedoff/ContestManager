using System.Web.Mvc;
using Core.Enums.RequestStatuses;
using Core.Managers;

namespace Front.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRegistrationManager registrationManager;

        public RegistrationController(IRegistrationManager registrationManager)
        {
            this.registrationManager = registrationManager;
        }

        [HttpGet]
        public ActionResult Index()
            => View();

        [HttpPost]
        public RegistrationRequestResult AddEmailRegistrationRequest(string userEmail)
            => registrationManager.AddEmailRegistrationRequest(userEmail);

        [HttpPost]
        public ConfirmRequestResult ConfirmEmailRegistrationRequest(string userName, string userEmail, string userPassword, string confirmationCode)
            => registrationManager.ConfirmEmailRegistrationRequest(userName, userEmail, userPassword, confirmationCode);
    }
}