using Microsoft.Practices.Unity;
using Readgress.Data;
using Readgress.Data.Contracts;
using Readgress.Data.Helpers;
using System.Web.Http;

namespace Readgress.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            //Unity by default picks the constructor with the most parameters. We have to tell unity to use a different one explicitly like this:
            container.RegisterType<RepositoryFactories>(new InjectionConstructor());
            container.RegisterType<IRepositoryProvider, RepositoryProvider>();
            container.RegisterType<IReadgressUow, ReadgressUow>(new HierarchicalLifetimeManager());

            return container;
        }
    }
}