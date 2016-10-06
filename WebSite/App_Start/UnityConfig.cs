using Data.Application;
using Data.Contexts;
using Data.Repositories;
using Data.Entities;
using Data.Service;
using Identity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using WebSite.Models;
using Identity.Entities;
using Identity.Repositories;

namespace WebSite
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion Unity Container

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<DbContext>(new PerRequestLifetimeManager());


            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductApp, ProductApp>();

            container.RegisterType<IUserRepository<int, IdentityUser, Identity.Entities.IdentityUserRole<int>, Identity.Entities.IdentityRoleClaim<int>>, UserRepository<int, IdentityUser, Identity.Entities.IdentityUserRole<int>, Identity.Entities.IdentityRoleClaim<int>>>();
            container.RegisterType<IRoleRepository<int, IdentityRole, IdentityUserRole<int>, IdentityRoleClaim<int>>, RoleRepository<int, IdentityRole, IdentityUserRole<int>, IdentityRoleClaim<int>>> ();
            container.RegisterType<AppUserManager>(new PerRequestLifetimeManager());
            container.RegisterType<AppSignInManager>(new PerRequestLifetimeManager());
            container.RegisterType<Identity.Stores.UserStore<int, IdentityUser, IdentityUserRole<int>,IdentityRoleClaim<int>>(new PerRequestLifetimeManager());
        }
    }
}