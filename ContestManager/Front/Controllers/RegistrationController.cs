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
        public RequestCreatingStatus CreateEmailRegistrationRequest(string userEmail)
            => registrationManager.CreateEmailRegistrationRequest(userEmail);

        [HttpPost]
        public RequestConfirmingStatus ConfirmEmailRegistrationRequest(string userName, string userEmail, string userPassword, string confirmationCode)
            => registrationManager.ConfirmEmailRegistrationRequest(userName, userEmail, userPassword, confirmationCode);
    }
}