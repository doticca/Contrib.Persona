using Orchard.ContentManagement.Records;

namespace Contrib.Persona.Models
{
    public class PersonaSettingsPartRecord : ContentPartRecord
    {
        public virtual bool RememberUser { get; set; }
        public virtual string ClassLogin { get; set; }
        public virtual string ClassLogout { get; set; }
        public virtual string VerifiedEmail { get; set; }
        public virtual string VerifiedSuperUser { get; set; }
    }
}