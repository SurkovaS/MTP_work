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
    using MTP1.Controllers.Abstract;
    using MTP1.Service.Factory;
    using MTP1.Service.Interface;
    using MTP1.Models;

    /// <summary>
    /// The actor dic controller.
    /// </summary>
    public class ProjectController : BaseController<Project>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public ProjectController(IBaseService<Project> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public ProjectController()
            : this(ProjectServiceFactory.Create())
        {
        }

        #endregion
    }
}