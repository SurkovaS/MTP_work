// --------------------------------------------------------------------------------------------------------------------
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
    public class ReliabilityEvaluationEntityService : BaseEntityService<ReliabilityEvaluation>
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
        protected override ObjectSet<ReliabilityEvaluation> EntitySet
        {
            get
            {
                return this.entities.ReliabilityEvaluation;
            }
        }

        #endregion
    }
}