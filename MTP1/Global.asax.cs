// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="">
//   
// </copyright>
// <summary>
//   The mvc application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    // Примечание: Инструкции по включению классического режима IIS6 или IIS7 
    // см. по ссылке http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// The mvc application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        #region Public Methods and Operators

        /// <summary>
        /// The register global filters.
        /// </summary>
        /// <param name="filters">
        /// The filters.
        /// </param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// The register routes.
        /// </summary>
        /// <param name="routes">
        /// The routes.
        /// </param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", 
                // Имя маршрута
                "{controller}/{action}/{id}", 
                // URL-адрес с параметрами
                new { controller = "Project", action = "Index", id = UrlParameter.Optional } // Параметры по умолчанию
                );
        }

        #endregion

        #region Methods

        /// <summary>
        /// The application_ start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        #endregion
    }
}