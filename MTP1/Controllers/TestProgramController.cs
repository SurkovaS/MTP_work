// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestProgramController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
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
    public class TestProgramController : BaseController<TestProgram>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProgramController"/> class. 
        /// Initializes a new instance of the <see cref="MTP1.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public TestProgramController(IBaseService<TestProgram> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProgramController"/> class. 
        /// Initializes a new instance of the <see cref="MTP1.Controllers.ProjectController"/> class.
        /// </summary>
        public TestProgramController()
            : this(TestProgramServiceFactory.Create())
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
        public ActionResult GetPrograms(int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(null, page, rows, search, sidx, sord);
        }

        private ActionResult FormJsonData(int? projectId, int page, int rows, string search, string sidx, string sord)
        {
            int testProgramCount = this.service.Get().Count();
            List<TestProgram> programs = this.service.Get().Where(a => a.Project == projectId || projectId == null)
                .ApplyPaging("Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                {
                    total = Paging.TotalPages(testProgramCount, rows),
                    page,
                    records = testProgramCount,
                    rows = (from m in programs
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title.ToStringWithDbNullCheck(), m.Description.ToStringWithDbNullCheck(), 
                                            m.Project1.Title.ToStringWithDbNullCheck(),
                                            m.BeginDatePlan.ToStringWithDbNullCheck(),
                                            m.EndDatePlan.ToStringWithDbNullCheck(),
                                            m.BeginDateActual.ToStringWithDbNullCheck(),
                                            m.EndDateActual.ToStringWithDbNullCheck()

                                        }
                                }).ToArray()
                };

            JsonResult jsonResult = this.Json(jsonData, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult GetProgramsForProject(int projectId, int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(projectId, page, rows, search, sidx, sord);
        }

        #endregion
    }
}