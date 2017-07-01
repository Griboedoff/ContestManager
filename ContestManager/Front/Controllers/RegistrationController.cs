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
        public RegistrationStatus CreateEmailRegistrationRequest(string userEmail)
            => registrationManager.CreateEmailRegistrationRequest(userEmail);

        [HttpPost]
        public RegistrationStatus ConfirmEmailRegistrationRequest(string userName, string userEmail, string userPassword, string confirmationCode)
            => registrationManager.ConfirmEmailRegistrationRequest(userName, userEmail, userPassword, confirmationCode);

        [HttpPost]
        public RegistrationStatus RegisterByVk(string userName, string userVkId)
            => registrationManager.RegisterByVk(userName, userVkId);
    }
}