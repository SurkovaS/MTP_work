// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using MTP1.Helpers;
using MTP1.Models;

namespace MTP1.Controllers
{
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
    public class ActorController : BaseController<Actor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public ActorController(IBaseService<Actor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public ActorController()
            : this(ActorServiceFactory.Create())
        {
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult GetActors(int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(null, page, rows, search, sidx, sord);
        }

        public ActionResult FormJsonData(int? useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            int actorCount = this.service.Get().Count();
            List<Actor> actors = this.service.Get().Where(a => a.UseCase == useCaseId || useCaseId == null)
                .ApplyPaging("ActorDic.Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                {
                    total = Paging.TotalPages(actorCount, rows),
                    page,
                    records = actorCount,
                    rows = (from m in actors
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                       {
                                            m.UseCase1.Title.ToStringWithDbNullCheck(), 
                                            m.ActorDic.Title.ToStringWithDbNullCheck(), 
                                            m.WeightCoefficientDic.Value.ToStringWithDbNullCheck(), 
                                            m.Quantity.ToStringWithDbNullCheck() 
                                        }
                                }).ToArray()
                };

            JsonResult jsonResult = this.Json(jsonData, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        public ActionResult GetActorsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(useCaseId, page, rows, search, sidx, sord);
        }
        #endregion
    }
}
