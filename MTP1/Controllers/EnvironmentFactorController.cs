// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MTP1.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using System;

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Models;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class EnvironmentFactorController : BaseController<EnvironmentFactor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public EnvironmentFactorController(IBaseService<EnvironmentFactor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public EnvironmentFactorController()
            : this(EnvironmentFactorServiceFactory.Create())
        {
        }

        #endregion

        #region Public Methods and Operators

        internal class EnvFactDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string WeightCoefficientValue { get; set; }
            public string DifficultyValue { get; set; }
        }

        public ActionResult GetEnvFactsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            var envFacts =
                this.service.Get().Where(a => a.UseCase == useCaseId).ToList().Select(
                    a => new EnvFactDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.EnvironmentFactorDic.ID,
                        Title = a.Title,
                        WeightCoefficientValue = a.WeightCoefficientValue,
                        DifficultyValue = a.DifficultyValue
                    }).ToList();

            List<EnvironmentFactorDic> allEnvFactorsDic = EnvironmentFactorDicServiceFactory.Create().Get().ToList();
            var missingEnvFactors = allEnvFactorsDic.Where(a => !envFacts.Any(b => b.ID == a.ID));
            envFacts.AddRange(missingEnvFactors.Select(a => new EnvFactDto { ID = a.ID, Title = a.Title }));

            int envFactCount = envFacts.Count();
            var envFactsWithPaging = envFacts.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(envFactCount, rows),
                    page,
                    records = envFactCount,
                    rows = (from m in envFactsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.WeightCoefficientValue,
                                            m.DifficultyValue
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}