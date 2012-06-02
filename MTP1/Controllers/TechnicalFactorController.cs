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
       
        public ActionResult GetTechFactsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            List<TechnicalFactor> techFacts = this.service.Get().Where(a => a.UseCase == useCaseId).ToList();
            List<TechnicalFactorDic> allTechFactors = TechnicalFactorDicServiceFactory.Create().Get().ToList();
            var missingTechFactors = allTechFactors.Where(a => !techFacts.Any(b => b.TechnicalFactorDic.ID == a.ID));
            
            techFacts.AddRange(missingTechFactors.Select(a => new TechnicalFactor { TechnicalFactorDic = a }));
            int techFactCount = techFacts.Count();
            var techFactsWithPaging = techFacts.AsQueryable().ApplyPaging("TechnicalFactorDic.Title", (page - 1) * rows, rows);
            
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
                                            m.TechnicalFactorDic.Title.ToStringWithDbNullCheck(), 
                                            m.WeightCoefficientDic == null ? string.Empty : 
                                                    m.WeightCoefficientDic.Value.ToStringWithDbNullCheck(), 
                                            m.PriorityDic == null ? string.Empty : 
                                                    m.PriorityDic.Title.ToStringWithDbNullCheck() 
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}