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
    public class Users_PMController : BaseController<Users_PM>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        public Users_PMController(IBaseService<Users_PM> service)
            : base(service)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActorDicController"/> class.
        /// </summary>
        public Users_PMController()
            : this(Users_PMServiceFactory.Create())
        {
        }

        #endregion
    }
}