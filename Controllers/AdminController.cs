using Contrib.Persona.BrowserID;
using Contrib.Persona.Models;
using Orchard;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.ContentManagement;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;
using Orchard.Security;
using Orchard.Themes;
using Orchard.Users.Events;
using Orchard.Users.Models;
using Orchard.Users.Services;
using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Linq;
using Orchard.Roles.Models;
using Contrib.Persona.Services;

namespace Contrib.Persona.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPersonaService _personaService;

        public AdminController(
            IPersonaService personaService
            )
        {
            _personaService = personaService;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }

        public Localizer T { get; set; }


        [HttpPost]
        public ActionResult PersonaVerifyEmail(string assertion)
        {
            var authentication = new BrowserIDAuthentication();
            var verificationResult = authentication.Verify(assertion);
            if (verificationResult.IsVerified)
            {
                string email = verificationResult.Email;
                return Json(new { email });
            }
            return Json(null);
        }

        [HttpPost]
        public ActionResult PersonaVerifyAdminEmail(string assertion)
        {
            var authentication = new BrowserIDAuthentication();
            var verificationResult = authentication.Verify(assertion);
            if (verificationResult.IsVerified)
            {
                string email = verificationResult.Email;
                if (!_personaService.UserExists(email))
                {
                    return Json(new { error = "User not found in this site", email = "", user = "" });
                }
                if (!_personaService.UserAuthorized(email))
                {
                    return Json(new { error = "User " + email + " not approved from admin", email = "", user = "" });
                }
                string username = _personaService.IsUserAdmin(email);
                if (!string.IsNullOrWhiteSpace(username))
                {
                    return Json(new { error = "", email = email, user = username });
                }
                else
                {
                    return Json(new { error = "User " + email + " is not an administrator or Super User", email = "", user = "" });
                }
            }
            return Json(new { error = "Persona could not verify this email", email = "", user= "" });
        }
    }
}