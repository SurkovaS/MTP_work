﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalFactorController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
    public class TechnicalFactorController : BaseController<TechnicalFactor>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalFactorController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public TechnicalFactorController(IBaseService<TechnicalFactor> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalFactorController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public TechnicalFactorController()
            : this(TechnicalFactorServiceFactory.Create())
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
        public ActionResult GetTechFactors(int page, int rows, string search, string sidx, string sord)
        {
            int currentPage = Convert.ToInt32(page) - 1;
            int techFactorsCount = this.service.Get().Count();
            var totalPages = (int) Math.Ceiling(techFactorsCount/(float) rows);
            List<TechnicalFactor> techFactors = this.service.Get().OrderBy(a =>a.TechnicalFactorDic.Title).Skip(0).Take(rows).ToList();

            try
            {
                var jsonData = new
                                   {
                                       total = totalPages,
                                       page,
                                       records = techFactorsCount,
                                       rows = (from m in techFactors
                                               select
                                                   new
                                                       {
                                                           id = m.ID,
                                                           cell =
                                                   new[]
                                                       {
                                                           m.UseCase1.Title /*.ToStringWithDbNullCheck()*/,
                                                           m.TechnicalFactorDic.Title /*.ToStringWithDbNullCheck()*/,
                                                           m.WeightCoefficientDic.Value.ToString() /*WithDbNullCheck*/,
                                                           m.PriorityDic.Title /*WithDbNullCheck*/,
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