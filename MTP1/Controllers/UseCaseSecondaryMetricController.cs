// --------------------------------------------------------------------------------------------------------------------
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
    public class UseCaseSecondaryMetricController : BaseController<UseCaseSecondaryMetric>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public UseCaseSecondaryMetricController(IBaseService<UseCaseSecondaryMetric> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public UseCaseSecondaryMetricController()
            : this(UseCaseSecondaryMetricServiceFactory.Create())
        {
        }
        internal class SMDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string SMValue { get; set; }
        }


        public ActionResult GetSMsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            var secMetrics =
                this.service.Get().Where(a => a.UseCase == useCaseId).ToList().Select(
                    a => new SMDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.SecondaryMetricDic.ID,
                        Title = a.Title,
                        SMValue = a.Value.ToString()
                    }).ToList();

            List<SecondaryMetricDic> allSMsDic = SecondaryMetricDicServiceFactory.Create().Get().ToList();
            var missingSMs = allSMsDic.Where(a => !secMetrics.Any(b => b.ID == a.ID));
            secMetrics.AddRange(missingSMs.Select(a => new SMDto { ID = a.ID, Title = a.Title }));

            int SMCount = secMetrics.Count();
            var SMsWithPaging = secMetrics.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(SMCount, rows),
                    page,
                    records = SMCount,
                    rows = (from m in SMsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.SMValue
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        

        #endregion
    }
}