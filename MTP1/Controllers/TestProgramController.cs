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
    using System;
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
                                            m.BeginDatePlan.DateToShortWithDbNullCheck(),
                                            m.EndDatePlan.DateToShortWithDbNullCheck(),
                                            m.BeginDateActual.DateToShortWithDbNullCheck(),
                                            m.EndDateActual.DateToShortWithDbNullCheck(),
                                            m.Users.Title,
                                            m.Project1.Title.ToStringWithDbNullCheck()

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


        public ActionResult EditRE(int programId)
        {
            if (Request.Form["oper"] != "edit")
            {
                throw new Exception("поддерживается только редактирование");
            }

            var program = this.service.Get().FirstOrDefault(a => a.ID == programId);
            if (program == null)
            {
                throw new Exception(string.Format("программа тестирования с id = {0} не найдена", programId));
            }

            int reIdParse;
            var reId = Request.Form["id"];
            if (!int.TryParse(reId, out reIdParse))
            {
                throw new Exception("не указан id показателя");
            }

            IBaseService<ReliabilityEvaluation> reService = ReliabilityEvaluationServiceFactory.Create();
            var re =
                reService.Get().FirstOrDefault(
                    a => a.Program == programId && a.ReliabilityEvaluationDic.ID == reIdParse);
            if (re == null)
            {
                re = new ReliabilityEvaluation { Program = programId, RE = reIdParse };
                reService.Add(re);
            }
            int reValueIdParse;
            var reValueId = Request.Form["Value"];
            if (int.TryParse(reValueId, out reValueIdParse))
            {
                re.Value = reValueIdParse;
            }

            reService.Save();
            RecalculateREMark(reService, program);
            return Json(true);
        }

        public ActionResult EditPMRE(int programId)
        {
            if (Request.Form["oper"] != "edit")
            {
                throw new Exception("поддерживается только редактирование");
            }

            var program = this.service.Get().FirstOrDefault(a => a.ID == programId);
            if (program == null)
            {
                throw new Exception(string.Format("программа тестирования с id = {0} не найдена", programId));
            }

            int pmreIdParse;
            var pmreId = Request.Form["id"];
            if (!int.TryParse(pmreId, out pmreIdParse))
            {
                throw new Exception("не указан id показателя");
            }

            IBaseService<PrimMetrRE> pmreService = PrimMetrREServiceFactory.Create();
            var pmre =
                pmreService.Get().FirstOrDefault(
                    a => a.Program == programId && a.PrimMetrREDic.ID == pmreIdParse);
            if (pmre == null)
            {
                pmre = new PrimMetrRE { Program = programId, PMRE = pmreIdParse };
                pmreService.Add(pmre);
            }
            int pmreValueIdParse;
            var pmreValueId = Request.Form["Value"];
            if (int.TryParse(pmreValueId, out pmreValueIdParse))
            {
                pmre.Value = pmreValueIdParse;
            }

            pmreService.Save();
            return Json(true);
        }

        private void RecalculateREMark(IBaseService<ReliabilityEvaluation> reService, TestProgram program)
        {
            var allREsForProgram = reService.Get().Where(a => a.Program == program.ID).ToList();
            double result = 0;
            foreach (var re in allREsForProgram)
            {
                result += (re.Value.Value == null ? 0 : re.Value.Value) * 5 / 100;

            }
            program.REMark = result;
            string good = "Достигнут желаемый уровень надежности";
            string bad = "Не достигнут желаемый уровень надежности";
            if (program.REMark>=program.REPlan)
            {
                program.Resolution = good.ToString();
            }
            else
            {
                program.Resolution = bad.ToString();
            }

            this.service.Save();
        }


        #endregion
    }
}