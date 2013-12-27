using System.Data;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;

namespace Contrib.Persona
{
    public class PersonaSettingsMigrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("PersonaSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<bool>("RememberUser")
                    .Column<string>("ClassLogin", c => c.WithDefault(".persona-login"))
                    .Column<string>("ClassLogout", c => c.WithDefault(".persona-logout"))
                    .Column<string>("VerifiedEmail", c => c.WithDefault(string.Empty))
                    .Column<string>("VerifiedSuperUser", c => c.WithDefault(string.Empty))
                );

            ContentDefinitionManager.AlterPartDefinition("PersonaSignInPart", part => part
                .WithDescription("Adds a Persona Sign In Button."));

            ContentDefinitionManager.AlterTypeDefinition("PersonaSignInWidget",
                cfg => cfg
                    .WithPart("PersonaSignInPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 1;
        }

    }
}