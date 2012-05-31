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
        public ActionResult GetActors(int page, int rows, string search, string sidx, string sord)
        {
            int actorsCount = this.service.Get().Count();
            List<Actor> actors = this.service.Get().ApplyPaging("ActorDic.Title", (page - 1) * rows, rows).ToList();
            var jsonData = new
            {
                total = Paging.TotalPages(actorsCount, rows),
                page,
                records = actorsCount,
                rows = (from m in actors
                        select
                            new
                            {
                                id = m.ID,
                                cell = new[]
                                    {
                                        m.UseCase1.Title.ToStringWithDbNullCheck(),
                                        m.ActorDic.Title.ToStringWithDbNullCheck(),
                                        m.WeightCoefficientDic.Value.ToStringWithDbNullCheck(),
                                        m.Quantity.ToStringWithDbNullCheck(),
                                    }
                            }).ToArray()
            };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}