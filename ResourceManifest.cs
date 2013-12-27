using Orchard.UI.Resources;

namespace Contrib.Persona {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineStyle("Persona")
                .SetUrl("persona.css");

            manifest.DefineScript("OrchardPersona")
                .SetDependencies("jQuery")
                .SetUrl("orchard-persona.js");

            // do not depend orchard persona so you can keep mozilla script on head and orchard script on foot
            manifest.DefineScript("Persona")
                .SetUrl("https://login.persona.org/include.js");

        }
    }
}
