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
    public class PrimMetrREController : BaseController<PrimMetrRE>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public PrimMetrREController(IBaseService<PrimMetrRE> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public PrimMetrREController()
            : this(PrimMetrREServiceFactory.Create())
        {
        }

        internal class PMREDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string PMREValue { get; set; }
        }

        public ActionResult GetPMREsForProgram(int programId, int page, int rows, string search, string sidx, string sord)
        {
            var PMREs =
                this.service.Get().Where(a => a.Program == programId).ToList().Select(
                    a => new PMREDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.PrimMetrREDic.ID,
                        Title = a.Title,
                        PMREValue = a.PMREValue
                    }).ToList();

            List<PrimMetrREDic> allPMREsDic = PrimMetrREDicServiceFactory.Create().Get().ToList();
            var missingPMREs = allPMREsDic.Where(a => !PMREs.Any(b => b.ID == a.ID));
            PMREs.AddRange(missingPMREs.Select(a => new PMREDto { ID = a.ID, Title = a.Title }));

            int PMRECount = PMREs.Count();
            var PMREsWithPaging = PMREs.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(PMRECount, rows),
                    page,
                    records = PMRECount,
                    rows = (from m in PMREsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.PMREValue
                                        }
                                }).ToArray()
                };


            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        
        #endregion
    }
}