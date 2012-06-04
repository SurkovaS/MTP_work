// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UseCaseController.cs" company="">
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
    /// The actor dic controller.
    /// </summary>
    public class UseCaseController : BaseController<UseCase>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public UseCaseController(IBaseService<UseCase> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public UseCaseController()
            : this(UseCaseServiceFactory.Create())
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
        public ActionResult GetUseCases(int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(null, page, rows, search, sidx, sord);
        }
        public ActionResult FormJsonData(int? programId,int page, int rows, string search, string sidx, string sord)
        {
            int useCaseCount = this.service.Get().Count();
            List<UseCase> useCases = this.service.Get().Where(a => a.TestProgram == programId || programId == null)
                .ApplyPaging("Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                {
                    total = Paging.TotalPages(useCaseCount, rows),
                    page,
                    records = useCaseCount,
                    rows = (from m in useCases
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                       {
                                            m.Title.ToStringWithDbNullCheck(), m.Description.ToStringWithDbNullCheck(), 
                                            m.PriorityDic == null ? string.Empty : m.PriorityDic.Title.ToStringWithDbNullCheck(), m.Ucp.ToStringWithDbNullCheck(), 
                                            m.ManHour.ToStringWithDbNullCheck(),
                                            m.Users.Title.ToStringWithDbNullCheck(),
                                            m.Project1.Title.ToStringWithDbNullCheck(), 
                                            m.TestProgram1.Title.ToStringWithDbNullCheck()
                                        }
                                        }).ToArray()
                    };

            JsonResult jsonResult = this.Json(jsonData, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        public ActionResult GetUseCasesForProgram(int programId, int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(programId, page, rows, search, sidx, sord);
        }

        public ActionResult EditTechFactor(int useCaseId)
        {
            if (Request.Form["oper"] != "edit")
            {
                throw new Exception("поддерживается только редактирование");
            }

            var useCase = this.service.Get().FirstOrDefault(a => a.ID == useCaseId);
            if (useCase == null)
            {
                throw new Exception(string.Format("use case с id = {0} не найден", useCaseId));
            }

            int techFactroIdParse;
            var techFactorId = Request.Form["id"];
            if (!int.TryParse(techFactorId, out techFactroIdParse))
            {
                throw new Exception("не указан id технического фактора");
            }

            IBaseService<TechnicalFactor> techFactorService = TechnicalFactorServiceFactory.Create();
            var tFactor =
                techFactorService.Get().FirstOrDefault(
                    a => a.UseCase == useCaseId && a.TechnicalFactorDic.ID == techFactroIdParse);
            if (tFactor == null)
            {
                tFactor = new TechnicalFactor { UseCase = useCaseId, TechnicalFactor1 = techFactroIdParse };
                techFactorService.Add(tFactor);
            }

            int wcIdParse;
            var wcId = Request.Form["WeightCoefficient"];
            if (int.TryParse(wcId, out wcIdParse))
            {
                tFactor.WeightCoefficient = wcIdParse;
            }

            int diffIdParse;
            var diffId = Request.Form["Difficulty"];
            if (int.TryParse(diffId, out diffIdParse))
            {
                tFactor.Difficulty = diffIdParse;
            }

            techFactorService.Save();
            //Response.Redirect(Request.RawUrl);
            RecalculateTechFactors(techFactorService, useCase);
            RecalculateUcp(useCase);
            RecalculateManHour(useCase);
            return Json(true);
        }

        public ActionResult EditEnvFactor(int useCaseId)
        {
            if (Request.Form["oper"] != "edit")
            {
                throw new Exception("поддерживается только редактирование");
            }

            var useCase = this.service.Get().FirstOrDefault(a => a.ID == useCaseId);
            if (useCase == null)
            {
                throw new Exception(string.Format("use case с id = {0} не найден", useCaseId));
            }

            int envFactroIdParse;
            var envFactorId = Request.Form["id"];
            if (!int.TryParse(envFactorId, out envFactroIdParse))
            {
                throw new Exception("не указан id технического фактора");
            }

            IBaseService<EnvironmentFactor> envFactorService = EnvironmentFactorServiceFactory.Create();
            var eFactor = envFactorService.Get().FirstOrDefault(
                a => a.UseCase == useCaseId &&
                a.EnvironmentFactorDic.ID == envFactroIdParse);
            if (eFactor == null)
            {
                eFactor = new EnvironmentFactor { UseCase = useCaseId, EnvironmentFactor1 = envFactroIdParse };
                envFactorService.Add(eFactor);
            }

            int envwcIdParse;
            var envwcId = Request.Form["WeightCoefficient"];
            if (int.TryParse(envwcId, out envwcIdParse))
            {
                eFactor.WeightCoefficient = envwcIdParse;
            }

            int envdiffIdParse;
            var envdiffId = Request.Form["Difficulty"];
            if (int.TryParse(envdiffId, out envdiffIdParse))
            {
                eFactor.Difficulty = envdiffIdParse;
            }

            envFactorService.Save();
            RecalculateEnvFactors(envFactorService, useCase);
            RecalculateUcp(useCase);
            RecalculateManHour(useCase);
            
            return Json(true);


        }


        public ActionResult EditActor(int useCaseId)
        {
            if (Request.Form["oper"] != "edit")
            {
                throw new Exception("поддерживается только редактирование");
            }

            var useCase = this.service.Get().FirstOrDefault(a => a.ID == useCaseId);
            if (useCase == null)
            {
                throw new Exception(string.Format("use case с id = {0} не найден", useCaseId));
            }

            int actroIdParse;
            var actorId = Request.Form["id"];
            if (!int.TryParse(actorId, out actroIdParse))
            {
                throw new Exception("не указан id технического фактора");
            }

            IBaseService<Actor> actorService = ActorServiceFactory.Create();
            var actor =
                actorService.Get().FirstOrDefault(
                    a => a.UseCase == useCaseId && a.ActorDic.ID == actroIdParse);
            if (actor == null)
            {
                actor = new Actor { UseCase = useCaseId, Actor1 = actroIdParse };
                actorService.Add(actor);
            }

            int actwcIdParse;
            var actwcId = Request.Form["WeightCoefficient"];
            if (int.TryParse(actwcId, out actwcIdParse))
            {
                actor.WeightCoefficient = actwcIdParse;
            }

            int actdiffIdParse;
            var actdiffId = Request.Form["Quantity"];
            if (int.TryParse(actdiffId, out actdiffIdParse))
            {
                actor.Quantity = actdiffIdParse;
            }

            actorService.Save();
            RecalculateActors(actorService, useCase);
            RecalculateUcp(useCase);
            RecalculateManHour(useCase);

            return Json(true);
        }

     
        private void RecalculateTechFactors(IBaseService<TechnicalFactor> techFactorService, UseCase useCase)
        {
            var allTechFactorsForUseCase = techFactorService.Get().Where(a => a.UseCase == useCase.ID).ToList();
            double result = 0;
            foreach (var technicalFactor in allTechFactorsForUseCase)
            {
                result += (technicalFactor.WeightCoefficientDic1 == null ? 0 : technicalFactor.WeightCoefficientDic1.Value)
                    * (technicalFactor.WeightCoefficientDic == null ? 0 : technicalFactor.WeightCoefficientDic.Value);
            }

            useCase.TF = (result / 100) + 0.6;
            this.service.Save();
        }


        private void RecalculateEnvFactors(IBaseService<EnvironmentFactor> envFactorService, UseCase useCase)
        {
            var allEnvFactorsForUseCase = envFactorService.Get().Where(a => a.UseCase == useCase.ID).ToList();
            double result = 0;
            foreach (var environmentFactor in allEnvFactorsForUseCase)
            {
                result += (environmentFactor.WeightCoefficientDic1 == null ? 0 : environmentFactor.WeightCoefficientDic1.Value)
                    * (environmentFactor.WeightCoefficientDic == null ? 0 : environmentFactor.WeightCoefficientDic.Value);
            }

            useCase.EF = 1.4 - (3 * result / 100);
            this.service.Save();
        }

        private void RecalculateActors(IBaseService<Actor> actorService, UseCase useCase)
        {
            var allActorsForUseCase = actorService.Get().Where(a => a.UseCase == useCase.ID).ToList();
            double result = 0;
            foreach (var actor in allActorsForUseCase)
            {
                result += (actor.WeightCoefficientDic.Value == null ? 0 : actor.WeightCoefficientDic.Value)
                    * (actor.Quantity.Value == null ? 0 : actor.Quantity.Value);
            
            }
            useCase.Actor = result;
            useCase.UUCP = useCase.Actor + useCase.Priority;
            this.service.Save();
        }

         private void RecalculateUcp(UseCase useCase)
         {
             double result = useCase.UUCP.Value * useCase.TF.Value * useCase.EF.Value;
             useCase.Ucp = result;
             this.service.Save();
         }


         private void RecalculateManHour(UseCase useCase)
        {
             double result = useCase.Ucp.Value * useCase.Users.PF.Value;
             useCase.ManHour = result;
             this.service.Save();
         }
        #endregion
    }
}