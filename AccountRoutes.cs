using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;
using Orchard;
using Orchard.Users.Models;
using Contrib.Persona.Routing;


namespace Contrib.Persona
{
    public class AccountRoutes : IRouteProvider
    {
        private readonly IPersonaConstraint _personaConstraint;
        public AccountRoutes(IPersonaConstraint personaConstraint)
        {
            _personaConstraint = personaConstraint;
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {

            return new[] {
                            new RouteDescriptor {
                                                Route = new Route(
                                                    "Admin/Settings/PersonaVerifyEmail/{assertion}",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Admin"},
                                                                                {"action", "PersonaVerifyEmail"}
                                                                            },
                                                    new RouteValueDictionary(),
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            new RouteDescriptor {
                                                Route = new Route(
                                                    "Admin/Settings/PersonaVerifyAdminEmail/{assertion}",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Admin"},
                                                                                {"action", "PersonaVerifyEmail"}
                                                                            },
                                                    new RouteValueDictionary(),
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            new RouteDescriptor {
                                                Priority = 11,
                                                Route = new Route(
                                                    "Users/Account/Persona",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Account"},
                                                                                {"action", "Persona"}
                                                                            },
                                                    new RouteValueDictionary(),
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            new RouteDescriptor {
                                                Priority = 11,
                                                Route = new Route(
                                                    "Users/Account/LogOn",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Account"},
                                                                                {"action", "LogOn"}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"path", _personaConstraint}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            new RouteDescriptor {
                                                Priority = 11,
                                                Route = new Route(
                                                    "Users/Account/LogOff",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Account"},
                                                                                {"action", "LogOff"}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"path", _personaConstraint}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            new RouteDescriptor {
                                                Priority = 11,
                                                Route = new Route(
                                                    "Users/Account/AccessDenied",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Account"},
                                                                                {"action", "AccessDenied"}
                                                                            },
                                                    new RouteValueDictionary {
                                                                {"path", _personaConstraint}
                                                            },
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                },
                            // route to cut all actions of other modules
                            new RouteDescriptor {
                                                Priority = 10,
                                                Route = new Route(
                                                    "Users/Account/{*path}",
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"},
                                                                                {"controller", "Account"},
                                                                                {"action", "AccessDenied"}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"path", _personaConstraint}
                                                                            },
                                                    new RouteValueDictionary {
                                                                                {"area", "Contrib.Persona"}
                                                                            },
                                                    new MvcRouteHandler())
                                                }
                         };
        }
    }
}