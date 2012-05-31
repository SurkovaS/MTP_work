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

    using MTP1.Controllers;
    using System;
    using MTP1.Controllers.Abstract;

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
            int currentPage = Convert.ToInt32(page) - 1;
            int actorsCount = this.service.Get().Count();
            var totalPages = (int)Math.Ceiling(actorsCount / (float)rows);
            List<Actor> actors = this.service.Get().OrderBy(a => a.ActorDic.Title).Skip(0).Take(rows).ToList();

            try
            {
                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = actorsCount,
                    rows = (from m in actors
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                                       {
                                                           m.UseCase1.Title /*.ToStringWithDbNullCheck()*/,
                                                           m.ActorDic.Title /*.ToStringWithDbNullCheck()*/,
                                                           m.WeightCoefficientDic.Value.ToString() /*WithDbNullCheck*/,
                                                           m.Quantity.ToString() /*WithDbNullCheck*/,
                                                          }
                                }).ToArray()
                };

                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}