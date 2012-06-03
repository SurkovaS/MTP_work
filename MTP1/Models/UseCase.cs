// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UseCase.cs" company="">
//   
// </copyright>
// <summary>
//   The actor dic.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Models
{
    using System.Linq;

    using MTP1.Models.Interface;
    using MTP1.Service.Factory;

    /// <summary>
    /// The actor dic.
    /// </summary>
    public partial class UseCase : IBase
    {
        private string wFactorList;

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

        public string WFactorAndDifficultyList
        {
            get
            {
                if (this.wFactorList == null)
                {
                    this.wFactorList = string.Empty;
                    var allWC = WeightCoefficientDicServiceFactory.Create().Get().OrderBy(a => a.Value).ToList();
                    foreach (var wc in allWC)
                    {
                        this.wFactorList += string.Format("{0}:\"{1}\",", wc.ID, wc.Value);
                    }
                    
                    this.wFactorList = "{" + this.wFactorList + "}";
                }

                return this.wFactorList;
            }
        }

        #endregion
    }
}