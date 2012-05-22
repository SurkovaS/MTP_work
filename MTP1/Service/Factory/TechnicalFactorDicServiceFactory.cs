// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDicServiceFactory.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic service factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Service.Factory
{
    using MTP1.Models;
    using MTP1.Service.Interface;

    /// <summary>
    /// The actor dic service factory.
    /// </summary>
    public static class TechnicalFactorDicServiceFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <returns>
        /// </returns>
        public static IBaseService<TechnicalFactorDic> Create()
        {
            return new TechnicalFactorDicEntityService();
        }

        #endregion
    }
}

