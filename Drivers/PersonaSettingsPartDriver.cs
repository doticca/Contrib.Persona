using Contrib.Persona.Models;
using Contrib.Persona.Services;
using Contrib.Persona.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Settings;
using Orchard.Users.Models;
using System;
using System.Linq;

namespace Contrib.Persona.Drivers
{
    public class PersonaSettingsPartDriver : ContentPartDriver<PersonaSettingsPart>
    {
        private const string TemplateName = "Parts/Persona.Settings";
        private readonly IOrchardServices _orchardServices;
        private readonly ISiteService _siteService;
        private readonly IPersonaService _personaService;
        public PersonaSettingsPartDriver(IOrchardServices orchardServices, ISiteService siteService, IPersonaService personaService)
        {
            _orchardServices = orchardServices;
            _siteService = siteService;
            _personaService = personaService;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "PersonaSettings"; } }

        protected override DriverResult Editor(PersonaSettingsPart part, dynamic shapeHelper)
        {
            string verifiedUserEmail = string.Empty;

            if (!string.IsNullOrWhiteSpace(part.VerifiedSuperUser))
            {
                    var user = _orchardServices.ContentManager.Query<UserPart, UserPartRecord>().Where(u => u.NormalizedUserName == part.VerifiedSuperUser).List().FirstOrDefault();
                    if (user != null) 
                        verifiedUserEmail = user.Email;
            }
            var viewModel = new PersonaSettingsViewModel
            {
                ClassLogin = part.ClassLogin,
                ClassLogout = part.ClassLogout,
                RememberUser = part.RememberUser,
                VerifiedSuperUserEmail = verifiedUserEmail,
                VerifiedEmail = part.VerifiedEmail,
                isUserAdmin = !string.IsNullOrWhiteSpace(_personaService.IsUserAdmin(part.VerifiedEmail))
            };
            return ContentShape("Parts_PersonaSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: viewModel, Prefix: Prefix))
                    .OnGroup("Persona"); 
        }

        protected override DriverResult Editor(PersonaSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            var viewModel = new PersonaSettingsViewModel();
            if (updater.TryUpdateModel(viewModel, Prefix, null, null))
            {
                part.ClassLogin = viewModel.ClassLogin;
                part.ClassLogout = viewModel.ClassLogout;
                part.RememberUser = viewModel.RememberUser;
                part.VerifiedEmail = viewModel.VerifiedEmail;
                part.VerifiedSuperUser = viewModel.VerifiedSuperUser;
            }
            return ContentShape("Parts_PersonaSettings_Edit",
                    () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: viewModel, Prefix: Prefix))
                    .OnGroup("Persona"); 
        }

        protected override void Exporting(PersonaSettingsPart part, ExportContentContext context)
        {
            var element = context.Element(part.PartDefinition.Name);

            element.SetAttributeValue("RememberUser", part.RememberUser);
            element.SetAttributeValue("ClassLogin", part.ClassLogin);
            element.SetAttributeValue("ClassLogout", part.ClassLogout);
        }

        protected override void Importing(PersonaSettingsPart part, ImportContentContext context)
        {
            var partName = part.PartDefinition.Name;

            part.Record.RememberUser = GetAttribute<bool>(context, partName, "RememberUser");
            part.Record.ClassLogin = GetAttribute<string>(context, partName, "ClassLogin");
            part.Record.ClassLogout = GetAttribute<string>(context, partName, "ClassLogout");
        }

        private TV GetAttribute<TV>(ImportContentContext context, string partName, string elementName)
        {
            string value = context.Attribute(partName, elementName);
            if (value != null)
            {
                return (TV)Convert.ChangeType(value, typeof(TV));
            }
            return default(TV);
        }
    }
}