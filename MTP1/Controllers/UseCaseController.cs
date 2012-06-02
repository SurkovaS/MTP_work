// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UseCaseController.cs" company="">
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
    public class UseCaseController : BaseController<UseCase>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public UseCaseController(IBaseService<UseCase> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UseCaseController"/> class. 
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public UseCaseController()
            : this(UseCaseServiceFactory.Create())
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
        public ActionResult GetUseCases(int page, int rows, string search, string sidx, string sord)
        {
            int useCasesCount = this.service.Get().Count();
            List<UseCase> useCases = this.service.Get().ApplyPaging("Title", (page - 1) * rows, rows).ToList();
            var jsonData =
                new
                    {
                        total = Paging.TotalPages(useCasesCount, rows), 
                        page, 
                        records = useCasesCount, 
                        rows = (from m in useCases
                                select
                                    new
                                        {
                                            id = m.ID, 
                                            cell =
                                    new[]
                                        {
                                            m.Title.ToStringWithDbNullCheck(), m.Description.ToStringWithDbNullCheck(), 
                                            m.Project1.Title.ToStringWithDbNullCheck(), 
                                            m.TestProgram1.Title.ToStringWithDbNullCheck(), 
                                            m.PriorityDic.Title.ToStringWithDbNullCheck(), m.Ucp.ToStringWithDbNullCheck(), 
                                            m.ManHour.ToStringWithDbNullCheck()
                                        }
                                        }).ToArray()
                    };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}