﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReliabilityEvaluationDic.cs" company="">
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
    public partial class ReliabilityEvaluation : IBase
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
                return this.ReliabilityEvaluationDic.Title.ToStringWithDbNullCheck();
            }
        }

        public string REValue
        {
            get
            {
                return this.Value == null
                           ? string.Empty
                           : this.Value.ToStringWithDbNullCheck();
            }
        }

        #endregion
    }
}