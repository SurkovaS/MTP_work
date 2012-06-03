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

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;
    using MTP1.Models;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class DefectDicController : BaseController<DefectDic>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public DefectDicController(IBaseService<DefectDic> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public DefectDicController()
            : this(DefectDicServiceFactory.Create())
        {
        }
        public ActionResult GetDefects(int page, int rows, string search, string sidx, string sord)
        {
            int defectsCount = this.service.Get().Count();
            List<DefectDic> defects = this.service.Get().ApplyPaging("Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                {
                    total = Paging.TotalPages(defectsCount, rows),
                    page,
                    records = defectsCount,
                    rows = (from m in defects
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                       {
                                            m.Title.ToStringWithDbNullCheck(), 
                                            m.Description.ToStringWithDbNullCheck()
                                        }
                                }).ToArray()
                };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}