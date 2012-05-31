﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestProgramController.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using MTP1.Controllers.Abstract;
    using MTP1.Models;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;

    /// <summary>
    /// The project controller.
    /// </summary>
    public class TestProgramController : BaseController<TestProgram>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProgramController"/> class. 
        /// Initializes a new instance of the <see cref="MTP1.Controllers.ProjectController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public TestProgramController(IBaseService<TestProgram> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProgramController"/> class. 
        /// Initializes a new instance of the <see cref="MTP1.Controllers.ProjectController"/> class.
        /// </summary>
        public TestProgramController()
            : this(TestProgramServiceFactory.Create())
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
        public ActionResult GetPrograms(int page, int rows, string search, string sidx, string sord)
        {
            int currentPage = Convert.ToInt32(page) - 1;
            int programsCount = this.service.Get().Count();
            var totalPages = (int)Math.Ceiling(programsCount / (float)rows);
            List<TestProgram> programs = this.service.Get().OrderBy(a => a.Title).Skip(0).Take(rows).ToList();

            try
            {
                var jsonData = new
                    {
                        total = totalPages, 
                        page, 
                        records = programsCount, 
                        rows = (from m in programs
                                select
                                    new
                                        {
                                            id = m.ID, 
                                            cell =
                                    new[]
                                        {
                                           m.Title, 
                                           m.Description,
                                           m.Project1.Title, 
                                           m.TestMethodDic.Title
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