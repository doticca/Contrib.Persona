using Orchard;
using System.Web.Routing;

namespace Contrib.Persona.Routing {
    public interface IPersonaConstraint : IRouteConstraint, ISingletonDependency {
    }
}