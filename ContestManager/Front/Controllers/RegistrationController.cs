using System.Web.Mvc;
using Core.Enums;
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
        public RegistrationRequestResult AddEmailRegistrationRequest(string userName, string userEmail, string userPassword)
            => registrationManager.AddEmailRegistrationRequest(userName, userEmail, userPassword);

        [HttpPost]
        public ConfirmRequestResult ConfirmEmailRegistration(string userEmail, string userConfirmCode)
            => registrationManager.ConfirmEmailRegistration(userEmail, userConfirmCode);
    }
}