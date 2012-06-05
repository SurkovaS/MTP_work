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

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    using MTP1.Models;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class ReliabilityEvaluationController : BaseController<ReliabilityEvaluation>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public ReliabilityEvaluationController(IBaseService<ReliabilityEvaluation> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public ReliabilityEvaluationController()
            : this(ReliabilityEvaluationServiceFactory.Create())
        {
        }

        internal class REDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string REValue { get; set; }
        }

        public ActionResult GetREsForProgram(int programId, int page, int rows, string search, string sidx, string sord)
        {
            var REs =
                this.service.Get().Where(a => a.Program == programId).ToList().Select(
                    a => new REDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.ReliabilityEvaluationDic.ID,
                        Title = a.Title,
                        REValue = a.REValue
                    }).ToList();

            List<ReliabilityEvaluationDic> allREsDic = ReliabilityEvaluationDicServiceFactory.Create().Get().ToList();
            var missingREs = allREsDic.Where(a => !REs.Any(b => b.ID == a.ID));
            REs.AddRange(missingREs.Select(a => new REDto { ID = a.ID, Title = a.Title }));

            int RECount = REs.Count();
            var REsWithPaging = REs.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(RECount, rows),
                    page,
                    records = RECount,
                    rows = (from m in REsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.REValue
                                        }
                                }).ToArray()
                };


            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}