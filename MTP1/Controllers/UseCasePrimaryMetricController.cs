﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicController.cs" company="">
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

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    using MTP1.Models;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class UseCasePrimaryMetricController : BaseController<UseCasePrimaryMetric>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public UseCasePrimaryMetricController(IBaseService<UseCasePrimaryMetric> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public UseCasePrimaryMetricController()
            : this(UseCasePrimaryMetricServiceFactory.Create())
        {
        }
        internal class PMDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string PMValue { get; set; }
        }


        public ActionResult GetPMsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            var primMetrics =
                this.service.Get().Where(a => a.UseCase == useCaseId).ToList().Select(
                    a => new PMDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.PrimaryMetricDic.ID,
                        Title = a.Title,
                        PMValue = a.Value.ToString()
                    }).ToList();

            List<PrimaryMetricDic> allPMsDic = PrimaryMetricDicServiceFactory.Create().Get().ToList();
            var missingPMs = allPMsDic.Where(a => !primMetrics.Any(b => b.ID == a.ID));
            primMetrics.AddRange(missingPMs.Select(a => new PMDto { ID = a.ID, Title = a.Title }));

            int PMCount = primMetrics.Count();
            var PMsWithPaging = primMetrics.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(PMCount, rows),
                    page,
                    records = PMCount,
                    rows = (from m in PMsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.PMValue
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}