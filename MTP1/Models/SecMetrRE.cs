// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrimaryMetricDic.cs" company="">
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

 
    public partial class SecMetrRE : IBase
    {
        #region Public Methods and Operators

        public bool CanBeDeleted()
        {
            return true;
        }

        public string Title
        {
            get
            {
                return this.SecMetrREDic.Title.ToStringWithDbNullCheck();
            }
        }

        public string SMREValue
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