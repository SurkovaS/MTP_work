﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActorDic.cs" company="">
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
    public partial class Actor : IBase
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
                return this.ActorDic.Title.ToStringWithDbNullCheck();
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

        public string QuantityValue
        {
            get
            {
                return this.Quantity == null
                           ? string.Empty
                           : this.Quantity.ToStringWithDbNullCheck();
            }
        }
        #endregion
    }
}