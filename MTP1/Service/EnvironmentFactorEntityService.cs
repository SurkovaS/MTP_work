﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentFactorDicEntityService.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic entity service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Service
{
    using System.Data.Objects;

    using MTP1.Models;
    using MTP1.Service.Abstract;

    /// <summary>
    /// The actor dic entity service.
    /// </summary>
    public class EnvironmentFactorEntityService : BaseEntityService<EnvironmentFactor>
    {
        #region Fields

        /// <summary>
        /// The entities.
        /// </summary>
        private readonly MtpDbEntities entities = new MtpDbEntities();

        #endregion

        #region Properties

        /// <summary>
        /// Gets EntitySet.
        /// </summary>
        protected override ObjectSet<EnvironmentFactor> EntitySet
        {
            get
            {
                return this.entities.EnvironmentFactor;
            }
        }

        #endregion
    }
}