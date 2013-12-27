using Contrib.Persona.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Users.Models;

namespace Contrib.Persona.Services {
    public interface IPersonaService : IDependency {
        string IsUserAdmin(UserPart user);
        string IsUserAdmin(string email);
        bool UserExists(string email);
        bool UserAuthorized(string email);
        PersonaSettingsPart PersonaSettings();
    }
}
