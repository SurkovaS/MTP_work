// --------------------------------------------------------------------------------------------------------------------
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
        public ActionResult GetEnvFactors(int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(null, page, rows, search, sidx, sord);
        }
        public ActionResult FormJsonData(int? useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            int envFactorCount = this.service.Get().Count();
            List<EnvironmentFactor> envFactors = this.service.Get().Where(a => a.UseCase == useCaseId || useCaseId == null)
                .ApplyPaging("EnvironmentFactorDic.Title", (page - 1) * rows, rows).ToList();

            var jsonData =
                new
                {
                    total = Paging.TotalPages(envFactorCount, rows),
                    page,
                    records = envFactorCount,
                    rows = (from m in envFactors
                            select
                                new
                                {
                                    id = m.ID,
                                    cell =
                            new[]
                                       {
                                            m.EnvironmentFactorDic.Title.ToStringWithDbNullCheck(), 
                                            m.WeightCoefficientDic == null ? string.Empty : 
                                                    m.WeightCoefficientDic.Value.ToStringWithDbNullCheck(), 
                                            m.WeightCoefficientDic1 == null ? string.Empty : 
                                                    m.WeightCoefficientDic1.Value.ToStringWithDbNullCheck() 
                                        }
                                }).ToArray()
                };

            JsonResult jsonResult = this.Json(jsonData, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        public ActionResult GetEnvFactorsForUseCase(int useCaseId, int page, int rows, string search, string sidx, string sord)
        {
            return this.FormJsonData(useCaseId, page, rows, search, sidx, sord);
        }
       

        #endregion
    }
}