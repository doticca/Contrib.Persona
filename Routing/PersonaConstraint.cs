using System;
using System.Web;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Users.Models;
using System.Linq;
using Orchard.Settings;
using Contrib.Persona.Services;
using Contrib.Persona.Models;

namespace Contrib.Persona.Routing {
    public class PersonaConstraint : IPersonaConstraint {
        private readonly IOrchardServices _orchardServices;
        private readonly ISiteService _siteService;
        private readonly IPersonaService _personaService;


        public PersonaConstraint(IOrchardServices orchardServices, ISiteService siteService, IPersonaService personaService)
        {
            _orchardServices = orchardServices;
            _siteService = siteService;
            _personaService = personaService;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) 
        {
            var personaSettings = _personaService.PersonaSettings();
            if (personaSettings == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(personaSettings.VerifiedEmail) || string.IsNullOrWhiteSpace(personaSettings.VerifiedSuperUser))
                {
                    return false;
                }
                else
                {
                    var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == personaSettings.VerifiedSuperUser).List().FirstOrDefault();
                    if (!_personaService.UserExists(personaSettings.VerifiedEmail))
                    {
                        return false;
                    }
                    if (user.Email != personaSettings.VerifiedEmail)
                    {
                        return false;
                    }
                    if (_personaService.IsUserAdmin(personaSettings.VerifiedEmail) != personaSettings.VerifiedSuperUser)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}