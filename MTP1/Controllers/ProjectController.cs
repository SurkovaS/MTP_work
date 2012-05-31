// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectController.cs" company="">
//   
// </copyright>
// <summary>
//   The project controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Models;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    /// <summary>
    /// The project controller.
    /// </summary>
    public class ProjectController : BaseController<Project>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public ProjectController(IBaseService<Project> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectController"/> class.
        /// </summary>
        public ProjectController()
            : this(ProjectServiceFactory.Create())
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get projects.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <param name="rows">
        /// The rows.
        /// </param>
        /// <param name="search">
        /// The search.
        /// </param>
        /// <param name="sidx">
        /// The sidx.
        /// </param>
        /// <param name="sord">
        /// The sord.
        /// </param>
        /// <returns>
        /// </returns>
        public ActionResult GetProjects(int page, int rows, string search, string sidx, string sord)
        {
            int projectsCount = this.service.Get().Count();
            List<Project> projects = this.service.Get().ApplyPaging("Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                    {
                        total = Paging.TotalPages(projectsCount, rows), 
                        page, 
                        records = projectsCount, 
                        rows = (from m in projects
                                select new
                                    {
                                        id = m.ID, 
                                        cell = new[]
                                            {
                                                m.Title, m.Version, 
                                                m.BeginDatePlaning.ToStringWithDbNullCheck(), 
                                                m.EndDatePlaning.ToStringWithDbNullCheck(), 
                                                m.BeginDateActual.ToStringWithDbNullCheck(), 
                                                m.EndDateActual.ToStringWithDbNullCheck()
                                            }
                                    }).ToArray()
                    };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}