// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalFactorController.cs" company="">
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
    public class TechnicalFactorController : BaseController<TechnicalFactor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalFactorController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public TechnicalFactorController(IBaseService<TechnicalFactor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalFactorController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public TechnicalFactorController()
            : this(TechnicalFactorServiceFactory.Create())
        {
        }

        #endregion

        #region Public Methods and Operators

        internal class TechFactDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string WeightCoefficientValue { get; set; }
            public string DifficultyValue { get; set; }
        }
       
        public ActionResult GetTechFactsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            var techFacts =
                this.service.Get().Where(a => a.UseCase == useCaseId).ToList().Select(
                    a => new TechFactDto
                        {
                            // эта бадяга затеивалась только чтобы взять ID из справочника
                            ID = a.TechnicalFactorDic.ID,
                            Title = a.Title,
                            WeightCoefficientValue = a.WeightCoefficientValue,
                            DifficultyValue = a.DifficultyValue
                        }).ToList();

            List<TechnicalFactorDic> allTechFactorsDic = TechnicalFactorDicServiceFactory.Create().Get().ToList();
            var missingTechFactors = allTechFactorsDic.Where(a => !techFacts.Any(b => b.ID == a.ID));
            techFacts.AddRange(missingTechFactors.Select(a => new TechFactDto { ID = a.ID, Title = a.Title }));

            int techFactCount = techFacts.Count();
            var techFactsWithPaging = techFacts.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                    {
                        total = Paging.TotalPages(techFactCount, rows),
                        page,
                        records = techFactCount,
                        rows = (from m in techFactsWithPaging
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