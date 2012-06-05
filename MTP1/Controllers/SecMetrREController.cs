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
    public class SecMetrREController : BaseController<SecMetrRE>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public SecMetrREController(IBaseService<SecMetrRE> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public SecMetrREController()
            : this(SecMetrREServiceFactory.Create())
        {
        }

        internal class SMREDto
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public string SMREValue { get; set; }
        }

        public ActionResult GetSMREsForProgram(int programId, int page, int rows, string search, string sidx, string sord)
        {
            var SMREs =
                this.service.Get().Where(a => a.Program == programId).ToList().Select(
                    a => new SMREDto
                    {
                        // эта бадяга затеивалась только чтобы взять ID из справочника
                        ID = a.SecMetrREDic.ID,
                        Title = a.Title,
                        SMREValue = a.SMREValue
                    }).ToList();

            List<SecMetrREDic> allSMREsDic = SecMetrREDicServiceFactory.Create().Get().ToList();
            var missingSMREs = allSMREsDic.Where(a => !SMREs.Any(b => b.ID == a.ID));
            SMREs.AddRange(missingSMREs.Select(a => new SMREDto { ID = a.ID, Title = a.Title }));

            int SMRECount = SMREs.Count();
            var SMREsWithPaging = SMREs.AsQueryable().ApplyPaging("Title", (page - 1) * rows, rows);

            var jsonData =
                new
                {
                    total = Paging.TotalPages(SMRECount, rows),
                    page,
                    records = SMRECount,
                    rows = (from m in SMREsWithPaging
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                        {
                                            m.Title,
                                            m.SMREValue
                                        }
                                }).ToArray()
                };


            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}