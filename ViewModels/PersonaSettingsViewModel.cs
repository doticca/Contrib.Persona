using Contrib.Persona.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contrib.Persona.ViewModels
{
    public class PersonaSettingsViewModel
    {
        public bool RememberUser { get; set; }
        public string ClassLogin { get; set; }
        public string ClassLogout { get; set; }
        public string VerifiedSuperUserEmail { get; set; }
        public string VerifiedSuperUser { get; set; }
        public string VerifiedEmail { get; set; }
        public bool isUserAdmin { get; set; }
    }
}