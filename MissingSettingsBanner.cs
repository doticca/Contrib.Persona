using System.Collections.Generic;
using System.Web.Mvc;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.UI.Admin.Notification;
using Orchard.UI.Notify;
using Contrib.Persona.Models;
using Orchard.Users.Models;
using System.Linq;
using Contrib.Persona.Services;

namespace Contrib.Persona
{
    public class MissingSettingsBanner : INotificationProvider
    {
        private readonly IOrchardServices _orchardServices;
        private readonly WorkContext _workContext;
        private readonly IPersonaService _personaService;

        public MissingSettingsBanner(
            IOrchardServices orchardServices, 
            IWorkContextAccessor workContextAccessor, 
            IPersonaService personaService
            )
        {
            _orchardServices = orchardServices;
            _workContext = workContextAccessor.GetContext();
            _personaService = personaService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public IEnumerable<NotifyEntry> GetNotifications()
        {
            var personaSettings = _orchardServices.WorkContext.CurrentSite.As<PersonaSettingsPart>();
            var superUser = _workContext.CurrentSite.SuperUser;
            var urlHelper = new UrlHelper(_workContext.HttpContext.Request.RequestContext);
            var url = urlHelper.Action("Persona", "Admin", new { Area = "Settings" });

            if (personaSettings == null)
            {
                yield return new NotifyEntry { Message = T("<a href=\"{0}\">Persona settings</a> needs to be configured.", url), Type = NotifyType.Warning };
            }
            else
            {
                if (string.IsNullOrWhiteSpace(personaSettings.VerifiedEmail) || string.IsNullOrWhiteSpace(personaSettings.VerifiedSuperUser))
                {
                    yield return new NotifyEntry { Message = T("<a href=\"{0}\">Persona settings</a> needs to be configured. You have to verify a site administrator e-mail before activating Persona.", url), Type = NotifyType.Warning };
                }
                else
                {
                    var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == personaSettings.VerifiedSuperUser).List().FirstOrDefault();
                    if (!_personaService.UserExists(personaSettings.VerifiedEmail))
                    {
                        yield return new NotifyEntry { Message = T("<a href=\"{0}\">Persona settings</a> needs to be configured. The verified user no longer exists.", url), Type = NotifyType.Warning };
                    }
                    if (user.Email != personaSettings.VerifiedEmail)
                    {
                        yield return new NotifyEntry { Message = T("<a href=\"{0}\">Persona settings</a> needs to be configured. The verified user changed his e-mail and need to follow the verification process again.", url), Type = NotifyType.Warning };
                    }
                    if (_personaService.IsUserAdmin(personaSettings.VerifiedEmail) != personaSettings.VerifiedSuperUser)
                    {
                        yield return new NotifyEntry { Message = T("<a href=\"{0}\">Persona settings</a> needs to be configured. The verified user is not an admin any more and need to follow the verification process again.", url), Type = NotifyType.Warning };
                    }
                }
            }
        }
    }
}