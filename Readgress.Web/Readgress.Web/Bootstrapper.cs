using System.Web.Http;
using System.Web.Mvc;
using GoogleBooksAPI;
using Microsoft.Practices.Unity;
using Readgress.Data;
using Readgress.Data.Contracts;
using Readgress.Data.Helpers;
using Readgress.PresentationModel.Utils;
using Readgress.Web.Areas.HelpPage.Controllers;

namespace Readgress.Web
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new Unity.Mvc3.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            //Unity by default picks the constructor with the most parameters. We have to tell unity to use a different one explicitly like this:
            container.RegisterType<RepositoryFactories>(new InjectionConstructor());
            container.RegisterType<IRepositoryProvider, RepositoryProvider>();
            container.RegisterType<IReadgressUow, ReadgressUow>(new PerResolveLifetimeManager());
            container.RegisterType<IFacebookLogin, FacebookLogin>();

            container.RegisterType<HelpController>(new InjectionConstructor());

            container.RegisterType<IDetails, Details>();
            return container;
        }
    }
}