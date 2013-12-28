using Contrib.Persona.Models;
using JetBrains.Annotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;

namespace Contrib.Persona.Handlers{
    [UsedImplicitly]
    public class PersonaSettingsPartHandler : ContentHandler{
        public PersonaSettingsPartHandler(IRepository<PersonaSettingsPartRecord> repository){
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<PersonaSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));

            OnInitializing<PersonaSettingsPart>((context, part) =>
            {
                part.RememberUser = true;
                part.ClassLogin = ".persona-login";
                part.ClassLogout = ".persona-logout";
                part.VerifiedEmail = string.Empty;
                part.VerifiedSuperUser = string.Empty;
            });
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Persona")));
        }
    }
}