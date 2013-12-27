using Orchard.ContentManagement;

namespace Contrib.Persona.Models
{
    public class PersonaSettingsPart : ContentPart<PersonaSettingsPartRecord>
    {
        public bool RememberUser
        {
            get { return Record.RememberUser; }
            set { Record.RememberUser = value; }
        }
        public string ClassLogin
        {
            get { return Record.ClassLogin; }
            set { Record.ClassLogin = value; }
        }
        public string ClassLogout
        {
            get { return Record.ClassLogout; }
            set { Record.ClassLogout = value; }
        }
        public string VerifiedEmail
        {
            get { return Record.VerifiedEmail; }
            set { Record.VerifiedEmail = value; }
        }
        public string VerifiedSuperUser
        {
            get { return Record.VerifiedSuperUser; }
            set { Record.VerifiedSuperUser = value; }
        }
    }
}