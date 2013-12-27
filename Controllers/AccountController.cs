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

namespace Contrib.Persona.Controllers
{
    [HandleError, Themed]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IOrchardServices _orchardServices;
        private readonly IUserEventHandler _userEventHandler;

        public AccountController(
            IAuthenticationService authenticationService,
            IMembershipService membershipService,
            IUserService userService,
            IOrchardServices orchardServices,
            IUserEventHandler userEventHandler
            )
        {
            _authenticationService = authenticationService;
            _membershipService = membershipService;
            _userService = userService;
            _orchardServices = orchardServices;
            _userEventHandler = userEventHandler;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        public ILogger Logger { get; set; }

        public Localizer T { get; set; }

        [AlwaysAccessible]
        public ActionResult AccessDenied()
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            var currentUser = _authenticationService.GetAuthenticatedUser();

            if (currentUser == null)
            {
                Logger.Information("Access denied to anonymous request on {0}", returnUrl);
                var shape = _orchardServices.New.PersonaLogOn().Title(T("Access Denied").Text);
                return new ShapeResult(this, shape);
            }

            Logger.Information("Access denied to user #{0} '{1}' on {2}", currentUser.Id, currentUser.UserName, returnUrl);

            _userEventHandler.AccessDenied(currentUser);

            return View();
        }

        [AlwaysAccessible]
        public ActionResult LogOn()
        {
            if (_authenticationService.GetAuthenticatedUser() != null)
                return Redirect("~/");
            var shape = _orchardServices.New.PersonaLogOn().Title(T("Log On").Text);
            return new ShapeResult(this, shape);
        }

        [HttpPost]
        [AlwaysAccessible]
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
        [AlwaysAccessible]
        [ValidateInput(false)]
        public ActionResult Persona(string assertion)
        {
            var rUrl = Request.QueryString["ReturnUrl"];
            var currentUser = _authenticationService.GetAuthenticatedUser();
            if (currentUser != null) _authenticationService.SignOut();


            var authentication = new BrowserIDAuthentication();
            var verificationResult = authentication.Verify(assertion);
            if (verificationResult.IsVerified)
            {
                var settings = _orchardServices.WorkContext.CurrentSite.As<PersonaSettingsPart>();
                string email = verificationResult.Email;
                var lowerEmail = email == null ? "" : email.ToLowerInvariant();
                var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == lowerEmail).List().FirstOrDefault();
                if (user == null)
                {
                    user = _membershipService.CreateUser(new CreateUserParams(lowerEmail, Guid.NewGuid().ToString(), lowerEmail, null, null, true)) as UserPart;
                    if (user == null)
                    {
                        return Json(null);
                    }
                }
                if (user.RegistrationStatus != UserStatus.Approved)
                {
                    return Json(null);
                }

                if (settings == null || !settings.RememberUser)
                {
                    _authenticationService.SignIn(user, false);
                }
                else
                {
                    _authenticationService.SignIn(user, true);
                }
                _userEventHandler.LoggedIn(user);
                string returnUrl = Url.Content(rUrl);

                return Json(new { returnUrl });
            }          
            return Json(null);
        }
        public ActionResult LogOff(string returnUrl)
        {
            var wasLoggedInUser = _authenticationService.GetAuthenticatedUser();
            _authenticationService.SignOut();
            if (wasLoggedInUser != null)
                _userEventHandler.LoggedOut(wasLoggedInUser);
            return this.RedirectLocal(returnUrl);
        }        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity is WindowsIdentity)
            {
                throw new InvalidOperationException(T("Windows authentication is not supported.").ToString());
            }
        }
    }
}