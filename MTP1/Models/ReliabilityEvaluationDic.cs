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
    using MTP1.Models.Interface;

    public partial class ReliabilityEvaluationDic : IBase
    {
        #region Public Methods and Operators

        public bool CanBeDeleted()
        {
            return true;
        }

        #endregion
    }
}