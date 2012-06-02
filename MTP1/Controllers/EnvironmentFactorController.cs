﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MTP1.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using System;

    using MTP.Controllers;

    using MTP1.Controllers.Abstract;
    using MTP1.Helpers;
    using MTP1.Models;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class EnvironmentFactorController : BaseController<EnvironmentFactor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public EnvironmentFactorController(IBaseService<EnvironmentFactor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public EnvironmentFactorController()
            : this(EnvironmentFactorServiceFactory.Create())
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
        public ActionResult GetEnvFactors(int page, int rows, string search, string sidx, string sord)
        {
            try
            {
                int envFactorsCount = this.service.Get().Count();
                List<EnvironmentFactor> envFactors = this.service.Get().ApplyPaging("EnvironmentFactorDic.Title", (page - 1) * rows, rows).ToList();

                var jsonData = new
                {
                    total = Paging.TotalPages(envFactorsCount, rows),
                    page,
                    records = envFactorsCount,
                    rows = (from m in envFactors
                            select
                                new
                                {
                                    id = m.ID,
                                    cell = new[]
                                            {
                                                m.UseCase1.Title.ToStringWithDbNullCheck(),
                                                m.EnvironmentFactorDic.Title.ToStringWithDbNullCheck(),
                                                m.WeightCoefficientDic.Value.ToStringWithDbNullCheck()//,
                                               // m.PriorityDic.Title.ToStringWithDbNullCheck()
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