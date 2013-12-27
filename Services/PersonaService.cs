using Orchard.ContentManagement;
using System.Text.RegularExpressions;
using Orchard.Utility.Extensions;
using System;
using System.Linq;
using Orchard;
using Orchard.Roles.Models;
using Orchard.Security;
using Orchard.Users.Models;
using Contrib.Persona.Models;
using Orchard.Settings;

namespace Contrib.Persona.Services {

    public class PersonaService : IPersonaService {
        private readonly IOrchardServices _orchardServices;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;
        public PersonaService(
            IOrchardServices orchardServices,
            IContentManager contentManager,
            ISiteService siteService
            ) 
        {
                _orchardServices = orchardServices;
                _contentManager = contentManager;
                _siteService = siteService;
        }

        public string IsUserAdmin(UserPart user)
        {
            if (user == null) return string.Empty;
            var superuser = _siteService.GetSiteSettings().SuperUser;
            if (user.As<UserRolesPart>().Roles.Contains("Administrator") || (!string.IsNullOrWhiteSpace(superuser) && superuser == user.NormalizedUserName))
            {
                return user.NormalizedUserName;
            }
            return string.Empty;
        }

        public string IsUserAdmin(string email)
        {
            var lowerEmail = email == null ? "" : email.ToLowerInvariant();
            var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>()
                .Where(u => u.Email == lowerEmail)
                .List()
                .FirstOrDefault();
            return IsUserAdmin(user);
        }

        public bool UserExists(string email)
        {
            var lowerEmail = email == null ? "" : email.ToLowerInvariant();
            var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == lowerEmail).List().FirstOrDefault();
            return user == null ? false : true;             
        }

        public bool UserAuthorized(string email)
        {
            var lowerEmail = email == null ? "" : email.ToLowerInvariant();
            var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.Email == lowerEmail).List().FirstOrDefault();
            return user.RegistrationStatus != UserStatus.Approved ? false : true;
        }

        public PersonaSettingsPart PersonaSettings()
        {
            return _contentManager.Query<PersonaSettingsPart, PersonaSettingsPartRecord>()
                    .List()
                    .FirstOrDefault();
        }
    }
}
