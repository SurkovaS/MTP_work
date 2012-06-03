// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalFactor.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Models
{
    using MTP.Controllers;

    using MTP1.Models.Interface;

    /// <summary>
    /// The actor dic.
    /// </summary>
    public partial class TechnicalFactor : IBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// The can be deleted.
        /// </summary>
        /// <returns>
        /// The can be deleted.
        /// </returns>
        public bool CanBeDeleted()
        {
            return true;
        }

        public string Title
        {
            get
            {
                return this.TechnicalFactorDic.Title.ToStringWithDbNullCheck();
            }
        }

        public string WeightCoefficientValue
        {
            get
            {
                return this.WeightCoefficientDic == null
                           ? string.Empty
                           : this.WeightCoefficientDic.Value.ToStringWithDbNullCheck();
            }
        }

        public string DifficultyValue
        {
            get
            {
                return this.WeightCoefficientDic1 == null
                           ? string.Empty
                           : this.WeightCoefficientDic1.Value.ToStringWithDbNullCheck();
            }
        }

        #endregion
    }
}