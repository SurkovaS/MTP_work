// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBase.cs" company="">
//   
// </copyright>
// <summary>
//   The i base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MTP1.Models.Interface
{
    /// <summary>
    /// The i base.
    /// </summary>
    public interface IBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ID.
        /// </summary>
        int ID { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The can be deleted.
        /// </summary>
        /// <returns>
        /// The can be deleted.
        /// </returns>
        bool CanBeDeleted();

        #endregion
    }
}