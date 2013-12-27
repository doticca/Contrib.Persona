using System;
using JetBrains.Annotations;
using Orchard.ContentManagement.Drivers;
using Contrib.Persona.Models;

namespace Contrib.Persona.Drivers {
    [UsedImplicitly]
    public class PersonaSignInDriver : ContentPartDriver<PersonaSignInPart> {

        public PersonaSignInDriver() {
        }

        protected override DriverResult Display(PersonaSignInPart part, string displayType, dynamic shapeHelper)
        {
            return
                ContentShape("Parts_Persona_SignIn",
                    () => shapeHelper.Parts_Persona_SignIn());

                
        }
    }
}